using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;
namespace HotelSo.CustomMiddwares
{
    public class ExceptionMiddleware
    {

        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                var tag = Guid.NewGuid().ToString();
                Log.Error($"Tag: {tag} - {exception}");

                await HandleExceptionAsync(httpContext, tag, exception.Message, exception.StackTrace);
               
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, string tag, string? message, string? stackTrace)
        {
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                Log.Error(contextFeature.Error.ToString());
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var moreDetails = string.Empty;

            if (AppDomain.CurrentDomain.GetData("IsDevelopment")?.ToString() == "True")
            {
                moreDetails = stackTrace;
            }

            return context.Response.WriteAsync(
                new Models.ErrorDetails
                {
                    Tag = tag,
                    StatusCode = context.Response.StatusCode,
                    Message = message,
                    StackTrace = moreDetails
                }.ToString()
            );
        }

    }
}
