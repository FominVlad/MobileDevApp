using System;
using System.ComponentModel.DataAnnotations;

namespace Chat.DAL.Models
{
    public class Message
    {
        public int MessageID { get; set; }

        [Required, MaxLength(7000)]
        public string Text { get; set; }

        [Required]
        public DateTime ReceivedDate { get; set; }

        [Required]
        public bool IsRead { get; set; }

        public int SenderID { get; set; }

        public User Sender { get; set; }

        [Required]
        public int ChatID { get; set; }

        public ChatEntity Chat { get; set; }
    }
}
