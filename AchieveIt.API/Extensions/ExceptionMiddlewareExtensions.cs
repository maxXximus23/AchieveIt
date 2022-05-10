using System;
using System.Net;
using AchieveIt.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AchieveIt.API.Extensions
{
    public class ExceptionMiddlewareExtensions : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            ConfigureExceptionHandler(context);
            context.ExceptionHandled = true;
        }
        private static void ConfigureExceptionHandler(ExceptionContext context)
        {
            Exception exception = context.Exception;

            switch (exception)
            {
                case NotFoundException:
                    SetExceptionResult(context, exception, HttpStatusCode.NotFound);
                    break;
                case ValidationException:
                    SetExceptionResult(context, exception, HttpStatusCode.BadRequest);
                    break;
                default:
                    SetExceptionResult(context, exception, HttpStatusCode.InternalServerError);
                    break;
            }
        }

        private static void SetExceptionResult(
            ExceptionContext context, 
            Exception exception, 
            HttpStatusCode statusCode)
        {
            context.Result = new JsonResult(exception.Message)
            {
                StatusCode = (int)statusCode
            };
        }
    }
}