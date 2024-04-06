using Microsoft.Extensions.Localization;
using System.Net;
using System.Text.Json;

namespace TimeAttendanceApp.Infrostructure.Errors
{
    public class ErrorHandingMiddleware(
        RequestDelegate next,
        //IStringLocalizer<ErrorHandingMiddleware> localizer,
        ILogger<ErrorHandingMiddleware> logger
        )
    {
        private static readonly Action<ILogger, string, Exception> LOGGER_MESSAGE =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            eventId: new EventId(id: 0, name: "ERROR"),
            formatString: "{Message}"
        );

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch ( Exception ex )
            {
                await HandleExceptionAsync(context, ex, logger/*, localizer*/);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ErrorHandingMiddleware> loger/*, IStringLocalizer<ErrorHandingMiddleware> localizer*/)
        {
            string? result;
            switch (exception )
            {
                case ServiceException se:
                    context.Response.StatusCode = (int)se.code;
                    result = exception.Message;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    LOGGER_MESSAGE(loger, "Unhandled Exception", exception);
                    result = JsonSerializer.Serialize(/*new { errors = "Internal Server Error" }*/ exception.Message);
                    break;
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync( result );
        }
    }
}
