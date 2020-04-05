using System;
using System.Collections.Generic;
using System.Text;

namespace MobileDevApp.Models
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
