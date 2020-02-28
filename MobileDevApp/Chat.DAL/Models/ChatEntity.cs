using System.Collections.Generic;

namespace Chat.DAL.Models
{
    public class ChatEntity
    {
        public int ChatID { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
