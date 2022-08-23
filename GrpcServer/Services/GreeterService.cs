using Grpc.Core;
using GrpcServer.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GrpcServer
{
    public class GreeterService : Greeter.GreeterBase
    {
        public object locker = new();  // lock
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override async Task SayHelloStream(IAsyncStreamReader<HelloRequest> requestStram,
            IServerStreamWriter<HelloReply> responseStram, ServerCallContext context)
        {
            CollectionPoint collection = new CollectionPoint();
            await collection.CollectionPointStart(100, 1000); //задаем размер коллекции координат

            while (true)
            {
                Task.Factory.StartNew(() =>
                {
                    Task.Factory.StartNew(() =>
                    {
                        collection.UpdatePoint();
                    }, TaskCreationOptions.AttachedToParent);

                    Task.Factory.StartNew(() =>
                    {
                        foreach (var item in collection.items)
                        {
                            lock (locker)
                            {
                                responseStram.WriteAsync(new HelloReply() { Id = item.Id, PointX = item.X, PointY = item.Y });
                            }
                        }
                    }, TaskCreationOptions.AttachedToParent);
                }).Wait();
            }

            //await foreach (var requst in requestStram.ReadAllAsync())
            //{
            //    responseStram.WriteAsync(new SetPoint() { Id = requst.Id, PointX = x, PointY = y }).Wait();
            //}
        }
    }
}
