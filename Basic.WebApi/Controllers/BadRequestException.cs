using System.Runtime.Serialization;

namespace Basic.WebApi.Controllers
{
    public class BadRequestException : BadHttpRequestException
    {
        public BadRequestException(string message)
            : base(message, 400) { }

        public BadRequestException(string message, Exception inner)
            : base(message, 400, inner) { }
    }
}
