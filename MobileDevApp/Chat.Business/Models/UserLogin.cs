namespace Chat.Business.Models
{
    public class UserLogin
    {
        public string Login { get; set; }
        public LoginType LoginType { get; set; }
        public string PasswordHash { get; set; }
    }

    public enum LoginType
    {
        PhoneNumber = 1,
        Email = 2
    }
}
