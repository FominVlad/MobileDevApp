namespace MobileDevApp.RemoteProviders.Models
{
    public class UserInfo : UserEdit
    {
        public int? UserID { get; set; } 

        public string AccessToken { get; set; }

        public UserInfo() { }

        public UserInfo(UserEdit baseInfo)
        {
            this.Name = baseInfo.Name;
            this.PhoneNumber = baseInfo.PhoneNumber;
            this.Email = baseInfo.Email;
            this.Bio = baseInfo.Bio;
            this.Image = baseInfo.Image;
        }

        public UserInfo(UserEdit baseInfo, int userID)
            :this(baseInfo)
        {
            this.UserID = userID;
        }
    }
}
