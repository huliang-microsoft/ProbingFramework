using BackendStub.Protos;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceStub.Controllers
{
    public class BasicController : Controller
    {
        public string Index()
        {
            return "This is the default action...";
        }

        [HttpGet]
        [Route("sendrequest")]
        public async Task<string> SendGrpcRequest()
        {
            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using var channel = GrpcChannel.ForAddress("https://localhost:5001",
                new GrpcChannelOptions { HttpHandler = httpHandler });

            var callInvoker = channel.Intercept(new ClientInterceptor());
            var client = new Greeter.GreeterClient(callInvoker);

            var name = "randomname + " + DateTime.Now.ToString();
            var request = new HelloRequest { Name = name };
            var cancellationToken = new CancellationToken();

            Console.WriteLine("Send a request");

            int i = 0;
            var response = client.SayHello(request);
            while (await response.ResponseStream.MoveNext(cancellationToken) && i < 5)
            {
                var reply = response.ResponseStream.Current;
                Console.WriteLine("Get the message: " + reply.Message);

                i++;
            }
                /*
                var reply = await client.SayHelloAsync(
                    new HelloRequest { Name = "GreeterClient" });

                Console.WriteLine("Get the message: " + reply.Message);
                */

            return "Sent a request";
        }
    }
}
