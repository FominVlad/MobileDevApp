using Chat.Business.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Chat.API.Filters
{
    public class ChatExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ChatException)
            {
                context.Result = new JsonResult(context.Exception.Message);
            }
            else
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                //context.Result = new JsonResult(context.Exception.GetType() + Environment.NewLine +
                //    context.Exception.Message + Environment.NewLine +
                //    context.Exception.StackTrace);
            }
        }
    }
}
