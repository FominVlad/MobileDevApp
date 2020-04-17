using System;
using System.Collections.Generic;
using System.Text;
using MobileDevApp.Models;

namespace MobileDevApp.Helpers
{
    public interface IGoogleAuth
    {
        void GetGoogleUser(SetGoogleUserInfo setGoogleUserInfo);
    }
}
