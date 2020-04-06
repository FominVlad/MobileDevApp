using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileDevApp.Models
{
    public class UserInfo
    {
        [PrimaryKey, Column("UserID")]
        public int? Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public byte[] Image { get; set; }

        public string AccessToken { get; set; }
    }
}
