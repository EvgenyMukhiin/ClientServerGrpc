using ClientRectangle.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ClientRectangle.Models
{
    public class RectangleItems : ViewModel
    {
        public int Id { get; set; }
        private int _X;
        public int X
        {
            get => _X;
            set => Set(ref _X, value);
        }
        private int _Y;
        public int Y
        {
            get => _Y;
            set => Set(ref _Y, value);
        }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Stroke { get; set; }
        public string Fill { get; set; }
        public string StrokeThickness { get; set; }
        private Thickness _Margin;
        public Thickness Margin
        {
            get => _Margin;
            set => Set(ref _Margin, value);
        }
        public RectangleItems(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
            Margin = new Thickness(x, y, 0, 0);
        }
        public async Task Move(int x, int y)
        {
           await Task.Run(() =>
            {
                Margin = new Thickness(x, y, 0, 0);
            });
        }
    }
    public class RectanglColections : ViewModel
    {
        private ObservableCollection<RectangleItems> _RectanglColection;
        public ObservableCollection<RectangleItems> RectanglColection
        {
            get => _RectanglColection;
            set => Set(ref _RectanglColection, value);
        }
        public RectanglColections()
        {
            RectanglColection = new ObservableCollection<RectangleItems>();
        }
        public void MoveItem(int id, int x, int y)
        {
            Task.Run(() =>
           {
               RectangleItems it = RectanglColection.FirstOrDefault(i => i.Id == id);
                //var it = GetItem(id).Result;
                if (it == null) //если нет добавляем
                {
                   Task.Run(() =>
                    {
                   Random rnd = new Random();
                   RectangleItems items = new RectangleItems(id, x, y)
                   {
                       Id = id,
                       Width = 50,
                       Height = 20,
                       Stroke = "Black",
                       Fill = $"#{rnd.Next(100000, 650000)}",
                       Margin = new Thickness(x, y, 0, 0)
                   };
                        Task.Run(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => RectanglColection.Add(items))); }).Wait();
                    });
               }
               else // если есть изменяем
                {
                    //Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => RectanglColection.Clear())).Wait();
                    Task.Run(() =>
                       {
                      //it.X = x;
                      //it.Y = y;
                      it.Margin = new Thickness(x, y, 0, 0);
                  }).Wait();
               }
           }).Wait();
        }
        //public async Task<RectangleItems> GetItem(int id)
        //{
        //   var it = await Task.Run(() =>
        //    {
        //        RectangleItems it = RectanglColection.FirstOrDefault(i => i.Id == id);
        //        return it;
        //    }).Result;

        //    return it;
        //}

    }
}
