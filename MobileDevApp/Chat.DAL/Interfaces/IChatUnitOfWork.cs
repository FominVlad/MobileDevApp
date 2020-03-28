using Chat.DAL.Models;
using System;

namespace Chat.DAL.Interfaces
{
    public interface IChatUnitOfWork : IDisposable
    {
        IChatRepository<Message> MessagesRepository { get; }
        IChatRepository<User> UsersRepository { get; }
        IChatRepository<ChatEntity> ChatsRepository { get; }
        IChatRepository<ChatUser> ChatUsersRepository { get; }
    }
}