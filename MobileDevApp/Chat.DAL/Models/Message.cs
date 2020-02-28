using System;
using System.ComponentModel.DataAnnotations;

namespace Chat.DAL.Models
{
    public class Message
    {
        public int MessageID { get; set; }

        [Required]
        public int ChatID { get; set; }

        public ChatEntity Chat { get; set; }

        [Required]
        public int SenderID { get; set; }

        public User Sender { get; set; }

        [Required]
        public int ReceiverID { get; set; }

        public User Receier { get; set; }

        [Required]
        public DateTime ReceivedDate { get; set; }
    }
}
