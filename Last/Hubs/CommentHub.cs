using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Last.Hubs
{
    public class CommentHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}