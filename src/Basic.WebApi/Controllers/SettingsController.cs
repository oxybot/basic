// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Basic.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Provides the API to manage the application configuration.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class SettingsController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsController"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    public SettingsController(Context context, IMapper mapper, ILogger<DefinitionsController> logger)
        : base(context, mapper, logger)
    {
    }

    /// <summary>
    /// Gets the configuration provider linked to the database.
    /// </summary>
    public DatabaseConfigurationProvider DatabaseProvider { get; }

    /// <summary>
    /// Retrieves the current configuration related to the email service.
    /// </summary>
    /// <param name="current">The current configuration as defined in the service layer.</param>
    /// <returns>The current configuration.</returns>
    [HttpGet]
    [Route("email")]
    [AuthorizeRoles(Role.Options)]
    public EmailServiceOptions GetEmail([FromServices] EmailServiceOptions current)
    {
        if (current is null)
        {
            throw new ArgumentNullException(nameof(current));
        }

        return current;
    }

    /// <summary>
    /// Updates the configuration related to the email service.
    /// </summary>
    /// <param name="options">The updated configuration as defined in the service layer.</param>
    [HttpPut]
    [Route("email")]
    [AuthorizeRoles(Role.Options)]
    public void PutEmail(EmailServiceOptionsForEdit options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        this.Save(EmailServiceOptions.Section, options);
    }

    /// <summary>
    /// Internal method to save any settings in the database.
    /// </summary>
    /// <param name="section">The section associated with the settings.</param>
    /// <param name="options">The options instance to store.</param>
    private void Save(string section, object options)
    {
        if (section is null)
        {
            throw new ArgumentNullException(nameof(section));
        }

        foreach (var property in options.GetType().GetProperties())
        {
            string key = property.Name;
            string value = property.GetValue(options)?.ToString();

            var settings = this.Context.Set<Setting>();

            var current = settings.SingleOrDefault(s => s.Section == section && s.Key == key);
            if (current == null)
            {
                // New entry
                settings.Add(new Setting() { Section = section, Key = key, Value = value });
            }
            else
            {
                // Update
                current.Value = value;
            }
        }

        this.Context.SaveChanges();
    }
}
