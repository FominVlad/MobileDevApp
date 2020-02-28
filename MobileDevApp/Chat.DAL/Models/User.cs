using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chat.DAL.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(1000)]
        public string Bio { get; set; }

        [Required, MaxLength(200)]
        public string QRCode { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
