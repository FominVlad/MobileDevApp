using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chat.DAL.Models
{
    public class ChatEntity
    {
        [Key]
        public int ChatID { get; set; }

        public virtual ICollection<ChatUser> Users { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
