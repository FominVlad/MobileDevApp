namespace MobileDevApp.RemoteProviders
{
    public static class Configuration
    {
        public static readonly int HttpWaitMs = 600;

        public static readonly int HttpRetryCount = 2;

        public static readonly string AuthHeaderKey = "Authentication";

        public static readonly string UserInfoBaseRoute = "api/chat-auth-user";

        public static readonly string UserInfoRegisterRoute = $"{UserInfoBaseRoute}/register";

        public static readonly string UserInfoLoginRoute = $"{UserInfoBaseRoute}/login";

        public static readonly string UserInfoEditRoute = $"{UserInfoBaseRoute}/edit";

        public static readonly string ChatInfoBaseRoute = "api/chat-info";

        public static readonly string ChatInfoAllChatsRoute = $"{ChatInfoBaseRoute}/all-chats";

        public static readonly string ChatInfoAllChatMessagesRoute = $"{ChatInfoBaseRoute}/all-chat-messages";

        public static readonly string ChatLiveMessagingUrl = "/chat";
    }
}
