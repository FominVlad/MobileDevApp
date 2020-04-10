using System.ComponentModel.DataAnnotations;

namespace Chat.Business.Models
{
    public class UserRegister : UserLogin
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }
}
