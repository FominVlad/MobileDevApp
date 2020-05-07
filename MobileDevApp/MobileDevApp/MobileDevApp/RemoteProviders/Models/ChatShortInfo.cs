using System;

namespace MobileDevApp.RemoteProviders.Models
{
    public class ChatShortInfo
    {
        public int ChatID { get; set; }

        public int PartnerID { get; set; }

        public string PartnerName { get; set; }

        public byte[] PartnerImage { get; set; }

        public MessageShortInfo LastMessage { get; set; }
    }
}
