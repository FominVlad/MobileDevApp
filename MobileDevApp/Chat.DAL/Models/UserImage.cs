using System.ComponentModel.DataAnnotations;

namespace Chat.DAL.Models
{
    public class UserImage
    {
        public int UserImageID { get; set; }

        public byte[] Image { get; set; }

        [Required]
        public int UserID { get; set; }

        public User User { get; set; }
    }
}
