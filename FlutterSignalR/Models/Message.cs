using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FlutterSignalR.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public int? Sender { get; set; }
        public int? Reciever { get; set; }
        public string Msg { get; set; }
        public DateTime? Date { get; set; }
    }
}
