using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendStub.Protos;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BackendStub.Services
{
    #region snippet
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override async Task SayHello(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            while (true)
            {
                await Task.Delay(1000);
                Console.WriteLine("Start to stream a response...");

                var response = new HelloReply()
                {
                    Message = "123"
                };

                await responseStream.WriteAsync(response);

                Console.WriteLine("Finished to stream a response...");
            }
            
        }
    }
    #endregion
}

