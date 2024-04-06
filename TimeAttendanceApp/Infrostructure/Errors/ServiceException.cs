using System.Net;

namespace TimeAttendanceApp.Infrostructure.Errors
{
    public class ServiceException: Exception
    {
        public HttpStatusCode code;
        ServiceException(HttpStatusCode code, string message)
            :base(message) 
        {
            this.code = code;
        }

        public static ServiceException NotFound(string message)
        {
            return new ServiceException(HttpStatusCode.NotFound, message);
        }

        public static ServiceException BadRequest(string message)
        {
            return new ServiceException(HttpStatusCode.BadRequest, message);
        }
    }
}
