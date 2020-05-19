using Chat.DAL.Interfaces;
using Chat.DAL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Patterns.Specification.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Chat.API.Auth
{
    public class ChatAuthenticationHandler : AuthenticationHandler<ChatAuthenticationScheme>
    {
        private const string AuthHeaderName = "Authentication";

        private readonly IChatUnitOfWork _chatUnitOfWork;

        public ChatAuthenticationHandler(
            IChatUnitOfWork chatUnitOfWork,
            IOptionsMonitor<ChatAuthenticationScheme> options, 
            ILoggerFactory logger, UrlEncoder encoder, 
            ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
            _chatUnitOfWork = chatUnitOfWork ?? throw new ArgumentNullException(nameof(chatUnitOfWork));
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(AuthHeaderName, out StringValues authHeader) || !authHeader.Any())
                return Task.FromResult(AuthenticateResult.Fail("Authentication header was not found"));

            var authToken = authHeader.First();

            var user = _chatUnitOfWork.UsersRepository.FirstOrDefault(
                new ExpressionSpecification<User>(u => u.Token.Equals(authToken)));

            if (user == null)
                return Task.FromResult(AuthenticateResult.Fail("Invalid token"));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.SerialNumber, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.MobilePhone, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Authentication, user.Token)
            };

            var identity = new ClaimsIdentity(claims, "Token", ClaimTypes.SerialNumber, string.Empty);

            var principal = new ClaimsPrincipal(identity: identity);

            var ticket = new AuthenticationTicket(
                principal: principal,
                authenticationScheme: Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
