// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Basic.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Provides test logger to help simulate logs and errors.
/// </summary>
/// <remarks>
/// Associated logger: <c>Basic.WebApi.Controllers.DebugController</c>.
/// </remarks>
[ApiController]
[Authorize]
[Route("[controller]")]
public class DebugController : BaseController
{
    /// <summary>
    /// The default test message.
    /// </summary>
    private const string TestMessage = "This is a test message";

    /// <summary>
    /// Initializes a new instance of the <see cref="DebugController"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    public DebugController(Context context, IMapper mapper, ILogger<DebugController> logger)
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
            this.Logger.Log(level, TestMessage);
        }
        else
        {
            this.Logger.Log(level, message);
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
                this.Logger.Log(level, e2, TestMessage);
            }
            else
            {
                this.Logger.Log(level, e2, message);
            }
        }
    }

    /// <summary>
    /// Provides sample error results.
    /// </summary>
    /// <param name="errorCode">The type of error to generate (400, 401, 403 or 404).</param>
    /// <returns>
    /// The error results associated with <paramref name="errorCode"/>.
    /// </returns>
    [HttpGet]
    [Produces("application/json")]
    [AuthorizeRoles(Role.User)]
    [Route("Errors")]
    public ListResult<UserForList> GetUsersWithError(string errorCode)
    {
        if (errorCode == "400")
        {
            // Returns a sample of a return due to parameters issues
            this.ModelState.AddModelError("sampleProperty", "Should be greater than 0");
            throw new InvalidModelStateException(this.ModelState);
        }
        else if (errorCode == "401")
        {
            // Returns a disconnection / not connected sample
            throw new UnauthorizedRequestException();
        }
        else if (errorCode == "403")
        {
            // Returns a not enough right sample
            throw new ForbiddenRequestException();
        }
        else if (errorCode == "404")
        {
            // Returns a not existing sample
            throw new NotFoundException("This entity doesn't exist");
        }

        return new ListResult<UserForList>(null) { Total = 0 };
    }
}
