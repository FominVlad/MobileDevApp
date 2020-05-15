using System;
using System.Collections.Generic;
using System.Text;

namespace MobileDevApp.RemoteProviders.Models
{
    class ChatShortInfoComparer : IEqualityComparer<ChatShortInfo>
    {
        public bool Equals(ChatShortInfo x, ChatShortInfo y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (x.ChatID != y.ChatID)
            {
                return false;
            }

            return true;
        }

        public int GetHashCode(ChatShortInfo obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            int hashProductName = obj.ChatID == null ? 0 : obj.ChatID.GetHashCode();

            return hashProductName ^ hashProductName;
        }
    }
}
