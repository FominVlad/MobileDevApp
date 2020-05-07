using System;

namespace MobileDevApp.RemoteProviders.Models
{
    public class MessageShortInfo
    {
        public string Text { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int SenderID { get; set; }
        public bool IsRead { get; set; }

        public static MessageInfo ToMessageInfo(MessageShortInfo message)
        {
            return new MessageInfo
            {
                Text = message.Text,
                ReceivedDate = message.ReceivedDate,
                SenderID = message.SenderID,
                IsRead = message.IsRead
            };
        }
    }
}
