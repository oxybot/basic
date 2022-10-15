using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Basic.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides test logger to help simulate logs and errors.
    /// </summary>
    /// <remarks>
    /// Associated logger: <c>Basic.WebApi.Controllers.LoggerController</c>.
    /// </remarks>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class LoggerController : BaseController
    {
        /// <summary>
        /// The default test message.
        /// </summary>
        private const string TestMessage = "This is a test message";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public LoggerController(Context context, IMapper mapper, ILogger<LoggerController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Adds a specific log message defined by its level and content.
        /// </summary>
        /// <param name="level">The log level of the message.</param>
        /// <param name="message">The content of the message, if any.</param>
        /// <remarks>
        /// The default message will be "This is a test message".
        /// </remarks>
        [HttpGet]
        [Produces("application/json")]
        [AuthorizeRoles(Role.User)]
        [Route("Message")]
        public void LogMessage(LogLevel level, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Logger.Log(level, TestMessage);
            }
            else
            {
                Logger.Log(level, message);
            }
        }

        /// <summary>
        /// Adds an exception log message defined by its level and content.
        /// </summary>
        /// <param name="level">The log level of the message.</param>
        /// <param name="message">The content of the message, if any.</param>
        /// <remarks>
        /// The default message will be "This is a test message".
        /// </remarks>
        [HttpGet]
        [Produces("application/json")]
        [AuthorizeRoles(Role.User)]
        [Route("Exception")]
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Used to help the technical setup")]
        [SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Used to help the technical setup")]
        public void LogException(LogLevel level, string message)
        {
            try
            {
                try
                {
                    var array = new string[] { null, null };
                    _ = array.Aggregate(0, (t, s) => t + s.Length);
                }
                catch (Exception e1)
                {
                    throw new SystemException(TestMessage, e1);
                }
            }
            catch (Exception e2)
            {
                if (string.IsNullOrEmpty(message))
                {
                    Logger.Log(level, e2, TestMessage);
                }
                else
                {
                    Logger.Log(level, e2, message);
                }
            }
        }
    }
}
