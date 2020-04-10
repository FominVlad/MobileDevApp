using MobileDevApp.RemoteProviders.Models;

namespace MobileDevApp.RemoteProviders.Interfaces
{
    public interface IUserService
    {
        UserInfo Info(int userId);
        UserInfo Info(string userSearchInfo);
        UserInfo Register(UserRegister newUser);
        UserInfo Login(UserLogin userLoginInfo);
        UserInfo Edit(UserEdit userEditInfo, string userAuthToken);
    }
}
