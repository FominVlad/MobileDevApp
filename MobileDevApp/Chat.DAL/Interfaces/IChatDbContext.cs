﻿using Chat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.DAL.Interfaces
{
    public interface IChatDbContext : IDisposable
    {
        DbSet<User> Users { get; }
        DbSet<ChatEntity> Chats { get; }
        DbSet<Message> Messages { get; }
        DbSet<ChatUser> ChatUsers { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
