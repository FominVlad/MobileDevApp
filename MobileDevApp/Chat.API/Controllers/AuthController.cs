using Chat.API.Models;
using Chat.Business.Interfaces;
using Chat.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;

namespace Chat.API.Controllers
{
    [Route("api/chat-auth-user")]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public AuthController(IUserManager userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpPost("register"),
        AllowAnonymous,
        Produces(MediaTypeNames.Application.Json),
        Consumes(MediaTypeNames.Application.Json),
        ProducesResponseType(typeof(UserInfo), StatusCodes.Status200OK),
        ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status500InternalServerError)]
        public ObjectResult Register([FromBody] UserRegister newUser)
        {
            UserInfo registeredUser = _userManager.RegisterUser(newUser);

            if (registeredUser == null)
                return StatusCode(
                    statusCode: StatusCodes.Status500InternalServerError,
                    value: new ErrorMessage
                    {
                        Message = "Failed to register user"
                    });

            return StatusCode(StatusCodes.Status200OK, registeredUser);
        }

        [HttpPost("login"),
        AllowAnonymous,
        Produces(MediaTypeNames.Application.Json),
        Consumes(MediaTypeNames.Application.Json),
        ProducesResponseType(typeof(UserInfo), StatusCodes.Status200OK),
        ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
        public ObjectResult Login([FromBody] UserLogin userLoginData)
        {
            UserInfo registeredUser = _userManager.GetUserInfo(userLoginData);

            if (registeredUser == null)
                return StatusCode(
                    statusCode: StatusCodes.Status400BadRequest,
                    value: new ErrorMessage
                    {
                        Message = "User was not found"
                    });

            return StatusCode(StatusCodes.Status200OK, registeredUser);
        }

        [HttpPost("edit"),
        Produces(MediaTypeNames.Application.Json),
        Consumes(MediaTypeNames.Application.Json),
        ProducesResponseType(typeof(UserInfo), StatusCodes.Status200OK),
        ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized),
        ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status500InternalServerError)]
        public ObjectResult Edit(UserEdit userEditInfo)
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userID))
                return StatusCode(
                    statusCode: StatusCodes.Status401Unauthorized,
                    value: new ErrorMessage
                    {
                        Message = "User was not authorized properly"
                    });

            UserInfo editedUser = new UserInfo(userEditInfo, userID);
            editedUser = _userManager.UpdateUserInfo(editedUser);

            if(editedUser == null)
                return StatusCode(
                    statusCode: StatusCodes.Status500InternalServerError,
                    value: new ErrorMessage
                    {
                        Message = "Failed to edit the userr info"
                    });

            return StatusCode(StatusCodes.Status200OK, editedUser);
        }

        //private async Task Authenticate(UserLogin userLoginModel, int userID)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimsIdentity.DefaultNameClaimType, userID.ToString()),
        //        new Claim(userLoginModel.LoginType.ToString(), userLoginModel.Login)
        //    };
        //    ClaimsIdentity identity = new ClaimsIdentity(claims);
        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        //}
    }
}
