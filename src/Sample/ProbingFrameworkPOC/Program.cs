using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BackendStub.Protos;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using ServiceStub.Protos;

namespace ProbingFrameworkPOC
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using var channel = GrpcChannel.ForAddress("https://localhost:5005",
                new GrpcChannelOptions { HttpHandler = httpHandler });

            var callInvoker = channel.Intercept(new ClientInterceptor());

            /*
            var client = new Greeter.GreeterClient(callInvoker);
            var request = new HelloRequest { Name = "GreeterClient" };
            */
            var client = new Probing.ProbingClient(callInvoker);
            var request = new ProbingRequest { Name = "ProbingClient" };

            var cancellationToken = new CancellationToken();

            while (true)
            {
                var response = client.MirrorRequests(request);
                while (await response.ResponseStream.MoveNext(cancellationToken))
                {
                    var reply = response.ResponseStream.Current;
                    Console.WriteLine("Get the message: " + reply.Message);
                }
                /*
                var reply = await client.SayHelloAsync(
                    new HelloRequest { Name = "GreeterClient" });

                Console.WriteLine("Get the message: " + reply.Message);
                */
            }
        }
    }
}
