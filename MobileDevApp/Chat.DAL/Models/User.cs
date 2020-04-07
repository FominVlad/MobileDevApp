using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chat.DAL.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(30)]
        public string PhoneNumber { get; set; }

        [EmailAddress, MaxLength(70)]
        public string Email { get; set; }

        [Required, MaxLength(1000)]
        public string PasswordHash { get; set; }

        [Required, MaxLength(1000)]
        public string Token { get; set; }

        [MaxLength(1000)]
        public string Bio { get; set; }

        public UserImage Image { get; set; }

        public virtual ICollection<ChatUser> Chats { get; set; }
    }
}
