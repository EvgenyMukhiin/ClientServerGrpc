using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;
using System;
using System.Threading.Tasks;

namespace GrpcServiceProvider
{
    public class ServiceProvider : IDisposable
    {
        private string ServiceUrl { get; set; }
        private Greeter.GreeterClient GreeterClient { get; set; }
        private Lazy<GrpcChannel> DefaultRpcChannel { get; set; }

        public ServiceProvider()
        {
            this.ServiceUrl = "https://localhost:5001";
            this.DefaultRpcChannel = new Lazy<GrpcChannel>(GrpcChannel.ForAddress(this.ServiceUrl));
        }
        public Greeter.GreeterClient GetGreeterClient() => this.GreeterClient ??= new Greeter.GreeterClient(this.DefaultRpcChannel.Value);
        #region IDisposable 
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.DefaultRpcChannel.IsValueCreated)
                    {
                        this.DefaultRpcChannel.Value.Dispose();
                    }
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion IDisposable
    }
}
