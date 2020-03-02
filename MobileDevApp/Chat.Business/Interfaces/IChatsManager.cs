using Chat.Business.Models;
using System.Collections.Generic;

namespace Chat.Business.Interfaces
{
    public interface IChatsManager
    {
        List<ChatShortInfo> GetAllChats(int userID);
        List<MessageShortInfo> GetChatMessages(int userID, int chatID, int? messagesCount = null);

        bool StoreMessage(MessageInfo newMessage);
    }
}
