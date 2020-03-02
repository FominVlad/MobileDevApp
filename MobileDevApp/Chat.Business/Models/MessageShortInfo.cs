using System;

namespace Chat.Business.Models
{
    public class MessageShortInfo
    {
        public string Text { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int SenderID { get; set; }
    }
}
