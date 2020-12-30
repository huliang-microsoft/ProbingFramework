using Grpc.Core;
using Microsoft.Extensions.Logging;
using ServiceStub.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStub.Service
{
    public class ProbingService : Probing.ProbingBase
    {
        private readonly ILogger<ProbingService> _logger;
        public ProbingService(ILogger<ProbingService> logger)
        {
            _logger = logger;
        }

        public override async Task MirrorRequests(ProbingRequest request, IServerStreamWriter<ProbingResponse> responseStream, ServerCallContext context)
        {
            MirrorRequestsQueue.mirrorRequest = true;

            while (true)
            {
                if (MirrorRequestsQueue.queue.Count() > 0)
                {
                    Console.WriteLine("Start to stream a mirrored request.");

                    var serviceRequest = MirrorRequestsQueue.queue.Dequeue();
                    var response = new ProbingResponse()
                    {
                        Message = serviceRequest.Name
                    };

                    await responseStream.WriteAsync(response);

                    Console.WriteLine("Finished to stream a mirrored request.");
                }
                else
                {
                    await Task.Delay(1000);
                }
            }
        }
    }
}
