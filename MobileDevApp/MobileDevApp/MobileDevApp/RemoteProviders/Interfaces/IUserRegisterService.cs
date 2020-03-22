using MobileDevApp.RemoteProviders.Models;

namespace MobileDevApp.RemoteProviders.Interfaces
{
    public interface IUserRegisterService
    {
        UserInfo Register(UserRegister newUser);
        UserInfo Login(UserLogin userLoginInfo);
        UserInfo Edit(UserEdit userEditInfo, string userAuthToken);
    }
}
