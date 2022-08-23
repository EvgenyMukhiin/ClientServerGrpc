using ClientRectangle.Infrastructure.Commands.Base;
using ClientRectangle.Models;
using ClientRectangle.ViewModels.Base;
using Grpc.Core;
using GrpcServiceProvider;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ClientRectangle.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        object locker = new();  // lock
        Random rnd = new Random();
        #region Заголовок окна
        private string _Title = "Rectangle Client";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion
        #region Модели и холст

        private RectanglColections _RectanglColection;
        public RectanglColections RectanglColection
        {
            get => _RectanglColection;
            set => Set(ref _RectanglColection, value);
        }

        private Canvas _CanvasApp;
        public Canvas CanvasApp
        {
            get => _CanvasApp;
            set => Set(ref _CanvasApp, value);
        }

        private bool _Start = true;
        public bool Start
        {
            get => _Start;
            set => Set(ref _Start, value);
        }

        #endregion
        #region Команды
        public ICommand StartCommand { get; }
        private bool CanStartCommandExecute(object p) => true;
        private void OnStartCommandExecuted(object p)
        {
            if (Start == false)
            {
                Start = true;
                Task.Run(() => ClientStream());
            }

            else
                Start = false;
        }
        #endregion
        public MainWindowViewModel()
        {
            #region Команды
            StartCommand = new ActionCommand(OnStartCommandExecuted, CanStartCommandExecute);
            #endregion
            RectanglColection = new RectanglColections();
            new Thread(() => { ClientStream(); }).Start();
        }
        public async void ClientStream()
        {
            using var channel = new ServiceProvider();
            var client = channel.GetGreeterClient();
            var call = client.SayHelloStream();

            var readTask = Task.Run(async () =>
            {
                await foreach (var response in call.ResponseStream.ReadAllAsync())
                {
                    Task.Factory.StartNew(() =>
                    {
                        Task.Factory.StartNew(() =>
                        {
                            var res = response;
                            lock (locker)
                            {
                                ColectionUpdate(res.Id, res.PointX, res.PointY);
                                //Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => ColectionUpdate(res.Id, res.PointX, res.PointY))).Wait();
                            }
                        }, TaskCreationOptions.AttachedToParent);
                    }).Wait();
                }
            });
            while (true)
            {
                if (Start == false)
                {
                    break;
                }
            }
            await call.RequestStream.CompleteAsync();
            await readTask;
        }
        public void ColectionUpdate(int id, int x, int y)
        {
            lock (locker)
            {
                Task.Run(() =>
                 {
                     Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => RectanglColection.MoveItem(id, x, y))).Wait();
                 }).Wait();
            }
        }
    }
}
