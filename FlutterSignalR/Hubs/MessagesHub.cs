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
            Console.WriteLine(Context.ConnectionId + " connected");
            await Clients.All.SendAsync("getid");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine(Context.ConnectionId + " disconnected");
            foreach (var id in map)
            {
                if (id.Value == Context.ConnectionId) map.Remove(id.Key);
            }

            await base.OnDisconnectedAsync(exception);
        }
        public async Task getid(int x)
        {
            map.Add(x,Context.ConnectionId);
            var messagelist = (from m in db.Message where m.Reciever == x select m).ToList();
            await Clients.Client(map[x]).SendAsync("idok", x);
            await Clients.Client(map[x]).SendAsync("newmessages", messagelist);
        }
        public async Task SendMessage(int sender,int receiver,string message)
        {
            Message m = new Message();
            m.Sender = sender;
            m.Reciever = receiver;
            m.Msg = message;
            m.Date = DateTime.Now;
            if (map[receiver] != null)
            {

                db.Message.Add(m);
                db.SaveChanges();
            }
            else
            {
                List<Message> messagelist = new List<Message>();
                messagelist.Add(m);
                await Clients.Client(map[receiver]).SendAsync("newmessages", messagelist);

            }
        }
    }
}
