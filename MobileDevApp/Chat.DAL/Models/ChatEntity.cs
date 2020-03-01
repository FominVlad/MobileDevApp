using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chat.DAL.Models
{
    public class ChatEntity
    {
        public int ChatID { get; set; }

        [Required]
        public int FirstMemberID { get; set; }

        public User FirstMember { get; set; }

        [Required]
        public int SecondMemberID { get; set; }

        public User SecondMember { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
