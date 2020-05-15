using MobileDevApp.RemoteProviders.Models;

namespace MobileDevApp.RemoteProviders.Interfaces
{
    public interface IUserService
    {
        UserInfo Info(int userId, string userAuthToken);
        UserInfo Info(string userSearchInfo, string userAuthToken);
        UserInfo Register(UserRegister newUser);
        UserInfo Login(UserLogin userLoginInfo);
        UserInfo Edit(UserEdit userEditInfo, string userAuthToken);
    }
}
