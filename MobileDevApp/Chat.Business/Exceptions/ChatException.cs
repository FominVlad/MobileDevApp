using System;

namespace Chat.Business.Exceptions
{
    public class ChatException : Exception
    {
        public ChatException(string message) : base(message) { }
    }
}
