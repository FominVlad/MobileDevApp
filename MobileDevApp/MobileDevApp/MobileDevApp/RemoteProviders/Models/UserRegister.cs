namespace MobileDevApp.RemoteProviders.Models
{
    public class UserRegister : UserLogin
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }
}
