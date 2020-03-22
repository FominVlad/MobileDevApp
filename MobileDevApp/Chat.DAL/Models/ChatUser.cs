using System.ComponentModel.DataAnnotations;

namespace Chat.DAL.Models
{
    public class ChatUser
    {
        public int ChatUserID { get; set; }

        [Required]
        public int ChatID { get; set; } 

        public ChatEntity Chat { get; set; }

        [Required]
        public int UserID { get; set; }

        public User User { get; set; }
    }
}
