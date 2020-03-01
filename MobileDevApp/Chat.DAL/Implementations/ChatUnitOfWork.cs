using Chat.DAL.Interfaces;
using Chat.DAL.Models;
using System;

namespace Chat.DAL.Implementations
{
    public class ChatUnitOfWork : IChatUnitOfWork
    {
        private readonly IChatDbContext _chatDbContext;

        public IChatRepository<Message> MessagesRepository { get; private set; }
        public IChatRepository<User> UsersRepository { get; private set; }
        public IChatRepository<ChatEntity> ChatsRepository { get; private set; }

        public ChatUnitOfWork(
            IChatDbContext chatDbContext,
            IChatRepository<Message> messagesRepository,
            IChatRepository<User> usersRepository,
            IChatRepository<ChatEntity> chatsRepository)
        {
            _chatDbContext = chatDbContext ?? throw new ArgumentNullException(nameof(chatDbContext));
            MessagesRepository = messagesRepository ?? throw new ArgumentNullException(nameof(messagesRepository));
            UsersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            ChatsRepository = chatsRepository ?? throw new ArgumentNullException(nameof(chatsRepository));
        }

        #region IDisposable

        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _chatDbContext.Dispose();
                }
                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
