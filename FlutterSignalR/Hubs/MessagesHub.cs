using FlutterSignalR.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterSignalR.Hubs
{
    public class MessagesHub : Hub
    {
        Dictionary<int, string> map = new Dictionary<int, string>();
        lanchatContext db = new lanchatContext();
        public async Task moveviewfromserver()
        {
            await Clients.All.SendAsync("recievenewposition", "Hello mofo\n");
            Console.WriteLine($"'Hello mofo' sent to app");
        }
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(Context.ConnectionId);
            await Clients.All.SendAsync("getid");
            await base.OnConnectedAsync();
        }
        public async Task getid(int x)
        {
            map.Add(x,Context.ConnectionId);
            var messagelist = (from m in db.Message where m.Reciever == x select m).ToList();
            await Clients.Client(map[x]).SendAsync("idok", x);
            await Clients.Client(map[x]).SendAsync("newmessages", messagelist);
        }
    }
}
