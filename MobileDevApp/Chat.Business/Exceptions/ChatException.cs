using System;

namespace Chat.Business.Exceptions
{
    public class ChatException : Exception
    {
        public ChatException(string message) : base(message) { }

        public ChatException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
