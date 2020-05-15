namespace Chat.API.Models
{
    public class MessageInput
    {
        public int ReceiverID { get; set; }
        public int? ChatID { get; set; }
        public string Text { get; set; }
    }
}
