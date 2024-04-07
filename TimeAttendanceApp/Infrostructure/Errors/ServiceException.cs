using System.Net;

namespace TimeAttendanceApp.Infrostructure.Errors
{
    public class ServiceException: Exception
    {
        public HttpStatusCode Code { get; }
        public object? Errors { get; }
        ServiceException(HttpStatusCode code, string message, object? errors = null)
            :base(message) 
        {
            this.Code = code;
            this.Errors = errors;
        }

        public static ServiceException NotFound(string message, object? errors = null)
        {
            return new ServiceException(HttpStatusCode.NotFound, message, errors);
        }

        public static ServiceException BadRequest(string message, object? errors = null)
        {
            return new ServiceException(HttpStatusCode.BadRequest, message, errors);
        }
    }
}
