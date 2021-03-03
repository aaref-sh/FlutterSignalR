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
        private static readonly Dictionary<int, string> map = new Dictionary<int, string>();
        private static readonly Dictionary<string, int> mp1 = new Dictionary<string, int>();
        lanchatContext db = new lanchatContext();
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(Context.ConnectionId + " connected");
            await Clients.All.SendAsync("getid");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine(Context.ConnectionId + " disconnected");
            try
            {
                map.Remove(mp1[Context.ConnectionId]);
                mp1.Remove(Context.ConnectionId);
            }
            finally
            {
                await base.OnDisconnectedAsync(exception);
            }
        }
        public async Task confid(int id)
        {
            map.Add(id, Context.ConnectionId);
            mp1.Add(Context.ConnectionId, id);
            var messagelist = (from m in db.Messages where m.Receiver == id select m).ToList();
            await Clients.Client(Context.ConnectionId).SendAsync("newmessages", messagelist);
            foreach (var x in messagelist) db.Messages.Remove(x);
            db.SaveChanges();
        }
        public async Task sendmessage(int sender,int receiver,string message)
        {
            Message m = new Message();
            m.Sender = sender;
            m.Receiver = receiver;
            m.Msg = message;
            m.Date = DateTime.Now;
            try
            {
                List<Message> messagelist = new List<Message> { m };
                await Clients.Client(map[receiver]).SendAsync("newmessages", messagelist);

            }
            catch
            {
                db.Messages.Add(m); db.SaveChanges();
            }
            
        }
        public string getidname(int id) => (from x in db.Users where x.Id == id select x.Name).First();
        public int signup(string name,string pass)
        {

            List<User> usrs = (from x in db.Users where x.Name == name select x).ToList();
            if (usrs.Count>0) return 0;

            User usr = new User();
            usr.Name = name;
            usr.Pass = pass;
            db.Users.Add(usr);
            db.SaveChanges();
            return usr.Id;
            
        }
        public int login(string name,string pass)
        {
            try
            {
                User usr = (from x in db.Users where x.Name == name && x.Pass == pass select x).First();
                if(usr!=null)return usr.Id;
            }
            catch{}
            return 0;
        }

    }
}
