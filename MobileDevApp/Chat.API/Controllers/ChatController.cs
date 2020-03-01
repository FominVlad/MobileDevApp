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
        private readonly IChatInfoProvider _chatInfoProvider;

        public ChatController(IChatInfoProvider chatInfoProvider)
        {
            _chatInfoProvider = chatInfoProvider ?? throw new ArgumentNullException(nameof(chatInfoProvider));
        }

        [HttpGet("all-chats"),
        Produces(MediaTypeNames.Application.Json),
        ProducesResponseType(typeof(List<ChatShortInfo>), StatusCodes.Status200OK),
        ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized)]
        public ObjectResult GetAllChats()
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userID))
                return StatusCode(
                    statusCode: StatusCodes.Status401Unauthorized,
                    value: new ErrorMessage
                    {
                        Message = "User was not authorized properly"
                    });

            List<ChatShortInfo> allUserChats = _chatInfoProvider.GetAllChats(userID);

            return StatusCode(StatusCodes.Status200OK, allUserChats);
        }

        [HttpGet("all-chat-messages/{chatID}"),
        Produces(MediaTypeNames.Application.Json),
        ProducesResponseType(typeof(List<MessageInfo>), StatusCodes.Status200OK),
        ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized)]
        public ObjectResult GetAllChatMesages(int chatID, [FromQuery] int? messagesCount)
        {
            if (!User.Identity.IsAuthenticated || !int.TryParse(User.Identity.Name, out int userID))
                return StatusCode(
                    statusCode: StatusCodes.Status401Unauthorized,
                    value: new ErrorMessage
                    {
                        Message = "User was not authorized properly"
                    });

            List<MessageInfo> allChatMessages = _chatInfoProvider.GetChatMessages(userID, chatID, messagesCount);

            return StatusCode(StatusCodes.Status200OK, allChatMessages);
        }
    }
}
