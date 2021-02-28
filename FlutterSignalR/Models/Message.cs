using System;
using System.Collections.Generic;

#nullable disable

namespace FlutterSignalR.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public int? Sender { get; set; }
        public int? Receiver { get; set; }
        public string Msg { get; set; }
        public DateTime? Date { get; set; }
    }
}
