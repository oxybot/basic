using System.Runtime.Serialization;

namespace Basic.WebApi.Controllers
{
    public class NotFoundException : BadHttpRequestException
    {
        public NotFoundException(string message)
            : base(message, 404) { }

        public NotFoundException(string message, Exception inner)
            : base(message, 404, inner) { }
    }
}
