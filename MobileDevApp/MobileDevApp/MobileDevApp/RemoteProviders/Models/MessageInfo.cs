namespace MobileDevApp.RemoteProviders.Models
{
    public class MessageInfo : MessageShortInfo
    {
        public int ReceiverID { get; set; }
        public int? ChatID { get; set; }
    }
}
