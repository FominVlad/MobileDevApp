using MobileDevApp.RemoteProviders.Models;

namespace MobileDevApp.RemoteProviders.Interfaces
{
    public interface IUserService
    {
        UserInfo Info(string userName);
        UserInfo Register(UserRegister newUser);
        UserInfo Login(UserLogin userLoginInfo);
        UserInfo Edit(UserEdit userEditInfo, string userAuthToken);
    }
}
