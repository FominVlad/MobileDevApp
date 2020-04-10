using System;

namespace MobileDevApp.RemoteProviders.Models
{
    public class ChatShortInfo
    {
        public int PartnerID { get; set; }

        public byte[] PartnerImage { get; set; }

        public MessageShortInfo LastMessage { get; set; }

        public DateTime? LastMessageDate { get; set; }
    }
}
