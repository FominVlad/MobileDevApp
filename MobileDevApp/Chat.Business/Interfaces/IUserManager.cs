using Chat.Business.Models;

namespace Chat.Business.Interfaces
{
    public interface IUserManager
    {

        UserInfo GetUserInfo(UserLogin userLoginData);
        UserInfo RegisterUser(UserRegister newUser);
        UserInfo UpdateUserInfo(UserInfo newUserInfo);
    }
}
