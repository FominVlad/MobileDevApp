using System;

namespace Chat.Business.Models
{
    public class ChatShortInfo
    {
        public int PartnerID { get; set; }

        public byte[] PartnerImage { get; set; }

        public string LastMessage { get; set; }

        public DateTime? LastMessageDate { get; set; }
    }
}
