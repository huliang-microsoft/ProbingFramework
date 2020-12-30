using BackendStub.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceStub
{
    public static class MirrorRequestsQueue
    {
        public static bool mirrorRequest = false;

        public static Queue<HelloRequest> queue = new Queue<HelloRequest>();
    }
}
