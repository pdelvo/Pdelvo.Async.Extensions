using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Pdelvo.Async.Extensions
{
    public static class TaskExtensions
    {
        public static Task<Socket> AcceptTaskAsync(this Socket owner)
        {
            return Task.Factory.FromAsync<Socket>(owner.BeginAccept, owner.EndAccept, null);
        }
        public static Task ConnectTaskAsync(this Socket owner, IPEndPoint endPoint)
        {
            return Task.Factory.FromAsync<IPEndPoint>(owner.BeginConnect, owner.EndConnect, endPoint, null, TaskCreationOptions.None);
        }
    }
}
