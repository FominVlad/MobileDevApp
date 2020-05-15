using Chat.API.Models;
using Chat.Business.Interfaces;
using Chat.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace Chat.API.Controllers
{
    [Route("api/chat-info")]
    public class ChatController : ControllerBase
    {
        private readonly IChatsManager _chatsManager;

        public ChatController(IChatsManager chatsManager)
        {
            _chatsManager = chatsManager ?? throw new ArgumentNullException(nameof(chatsManager));
        }

        [HttpGet("all-chats"),
        Produces(MediaTypeNames.Application.Json),
        ProducesResponseType(typeof(List<ChatShortInfo>), StatusCodes.Status200OK),
        ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized)]
        public ObjectResult GetAllChats()
        {
            if (User?.Identity == null || 
                !User.Identity.IsAuthenticated || 
                !int.TryParse(User.Identity.Name, out int userID))
                return StatusCode(
                    statusCode: StatusCodes.Status401Unauthorized,
                    value: new ErrorMessage
                    {
                        Message = "User was not authorized properly"
                    });

            List<ChatShortInfo> allUserChats = _chatsManager.GetAllChats(userID);

            return StatusCode(StatusCodes.Status200OK, allUserChats);
        }

        [HttpGet("all-chat-messages/{chatID}"),
        Produces(MediaTypeNames.Application.Json),
        ProducesResponseType(typeof(List<MessageShortInfo>), StatusCodes.Status200OK),
        ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized)]
        public ObjectResult GetAllChatMesages(int chatID, 
            [FromQuery] int? skipCount, 
            [FromQuery] int? takeCount)
        {
            if (User?.Identity == null || 
                !User.Identity.IsAuthenticated || 
                !int.TryParse(User.Identity.Name, out int userID))
                return StatusCode(
                    statusCode: StatusCodes.Status401Unauthorized,
                    value: new ErrorMessage
                    {
                        Message = "User was not authorized properly"
                    });

            List<MessageShortInfo> allChatMessages = 
                _chatsManager.GetChatMessages(userID, chatID, skipCount, takeCount);

            return StatusCode(StatusCodes.Status200OK, allChatMessages);
        }
    }
}
