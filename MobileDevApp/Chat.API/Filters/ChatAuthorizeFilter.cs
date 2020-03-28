using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.API.Filters
{
    public class ChatAuthorizeFilter : AuthorizeFilter
    {
        public ChatAuthorizeFilter(AuthorizationPolicy policy) : base(policy) { }

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType().Equals(typeof(AllowAnonymousAttribute))))
                return Task.FromResult(0);

            return base.OnAuthorizationAsync(context);
        }
    }
}
