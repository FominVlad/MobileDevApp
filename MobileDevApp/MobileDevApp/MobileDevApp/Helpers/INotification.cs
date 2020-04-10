using System;
using System.Collections.Generic;
using System.Text;

namespace MobileDevApp.Helpers
{
    public interface INotification
    {
        void CreateNotification(String title, String message);
    }
}
