using Chat.API.Auth;
using Chat.API.Filters;
using Chat.API.Hubs;
using Chat.Business.Implementations;
using Chat.Business.Interfaces;
using Chat.DAL.Implementations;
using Chat.DAL.Interfaces;
using Chat.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Chat.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ChatDbContext>(options =>
                options.UseSqlServer(Configuration["Data:ChatDB:ConnectionString"],
                    op => op.EnableRetryOnFailure()));
            services.AddScoped<IChatDbContext, ChatDbContext>();

            services.AddScoped<IChatRepository<ChatEntity>>(sp =>
                new ChatBaseRepository<ChatEntity>(sp.GetRequiredService<IChatDbContext>(), db => db.Chats));
            services.AddScoped<IChatRepository<User>>(sp =>
                new ChatBaseRepository<User>(sp.GetRequiredService<IChatDbContext>(), db => db.Users));
            services.AddScoped<IChatRepository<Message>>(sp =>
                new ChatBaseRepository<Message>(sp.GetRequiredService<IChatDbContext>(), db => db.Messages));
            services.AddScoped<IChatRepository<ChatUser>>(sp =>
                new ChatBaseRepository<ChatUser>(sp.GetRequiredService<IChatDbContext>(), db => db.ChatUsers));
            services.AddScoped<IChatUnitOfWork, ChatUnitOfWork>();

            services.AddScoped<IChatsManager, ChatsManager>();
            services.AddScoped<IUserManager>(sp =>
            {
                return new UserManager(
                    chatUnitOfWork: sp.GetService<IChatUnitOfWork>(),
                    secret: Configuration["Auth:Secret"]);
            });

            services.AddAuthentication(ChatAuthenticationScheme.SchemeName)
               .AddScheme<ChatAuthenticationScheme, ChatAuthenticationHandler>(ChatAuthenticationScheme.SchemeName, null);

            services.AddControllers(options => 
            {
                var policy = new AuthorizationPolicyBuilder()
                   .AddAuthenticationSchemes(new[] { ChatAuthenticationScheme.SchemeName })
                   .RequireAuthenticatedUser()
                   .Build();

                options.Filters.Add(new ChatAuthorizeFilter(policy));
            });
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Chat.API",
                    Version = "v1",
                    Description = "A software product that is designed to food delivery.",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.OperationFilter<SwaggerAuthHeaderFilter>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "chat-api/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/chat-api/swagger/v1/swagger.json", "Chat.API");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
