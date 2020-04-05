using System;
using System.Collections.Generic;
using System.Text;

namespace MobileDevApp.Models
{
    class UserRegister : UserLogin
    {
        public string Name { get; set; }

        public string QRCode { get; set; }
    }
}
