using Microsoft.AspNetCore.Authentication;

namespace Chat.API.Auth
{
    public class ChatAuthenticationScheme : AuthenticationSchemeOptions
    {
        public static string SchemeName => "ChatAuthenticationScheme";
    }
}
