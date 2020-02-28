using Chat.DAL.Interfaces;
using Chat.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.DAL.Implementations
{
    public class ChatDbContext : DbContext, IChatDbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<ChatEntity> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}
