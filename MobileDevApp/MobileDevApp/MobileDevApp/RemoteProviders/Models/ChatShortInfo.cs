using System;
using System.Collections;

namespace MobileDevApp.RemoteProviders.Models
{
    public class ChatShortInfo
    {
        public int? ChatID { get; set; }

        public int PartnerID { get; set; }

        public string PartnerName { get; set; }

        public byte[] PartnerImage { get; set; }

        public MessageShortInfo LastMessage { get; set; }

        public override bool Equals(object obj)
        {
            ChatShortInfo chat = (ChatShortInfo)obj;

            if(chat == null)
            {
                return false;
            }

            if(chat.ChatID != this.ChatID)
            {
                return false;
            }

            return true;
        }
    }
}
