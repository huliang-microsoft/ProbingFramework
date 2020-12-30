using BackendStub.Protos;
using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Grpc.Core.Interceptors.Interceptor;

namespace ServiceStub
{
    public class ClientInterceptor : Interceptor
    {
        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            LogCall(context.Method);


            if (request is HelloRequest)
            {
                var helloRequest = request as HelloRequest;
                // Console.WriteLine(helloRequest.Name);

                // If there is a activate request, start to mirror the request.
                if (MirrorRequestsQueue.mirrorRequest)
                {
                    MirrorRequestsQueue.queue.Enqueue(helloRequest);
                }
            }
            var responseContinuation = continuation(request, context);

            return responseContinuation;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
          TRequest request,
          ClientInterceptorContext<TRequest, TResponse> context,
          AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            LogCall(context.Method);

            return continuation(request, context);
        }

        private void LogCall<TRequest, TResponse>(Method<TRequest, TResponse> method)
          where TRequest : class
          where TResponse : class
        {
            var initialColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Starting call. Type: {method.Type}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
            Console.ForegroundColor = initialColor;
        }
    }
}
