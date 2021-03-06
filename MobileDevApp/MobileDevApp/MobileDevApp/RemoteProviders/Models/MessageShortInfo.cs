﻿using System;

namespace MobileDevApp.RemoteProviders.Models
{
    public class MessageShortInfo
    {
        public string Text { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int SenderID { get; set; }
        public bool IsRead { get; set; }

        public MessageInfo ToMessageInfo()
        {
            return new MessageInfo
            {
                Text = this.Text,
                ReceivedDate = this.ReceivedDate,
                SenderID = this.SenderID,
                IsRead = this.IsRead
            };
        }
    }
}
