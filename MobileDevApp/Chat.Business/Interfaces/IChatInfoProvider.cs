using Chat.Business.Models;
using System.Collections.Generic;

namespace Chat.Business.Interfaces
{
    public interface IChatInfoProvider
    {
        List<ChatShortInfo> GetAllChats(int userID);
        List<MessageInfo> GetChatMessages(int userID, int chatID, int? messagesCount = null);
    }
}
