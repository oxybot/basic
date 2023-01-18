// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text;
using System.Text.Json;

namespace Basic.WebApi.Framework;

/// <summary>
/// Provides helper methods for healthcheck management.
/// </summary>
public static class HealthCheck
{
    /// <summary>
    /// Provides a json based responsed to replace the standard text only approach.
    /// </summary>
    /// <param name="context">The current http context.</param>
    /// <param name="healthReport">The health report.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// Code based on Microsoft standard documentation.
    /// </remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-7.0" />
    public static Task ResponseWriterAsync(HttpContext context, HealthReport healthReport)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (healthReport is null)
        {
            throw new ArgumentNullException(nameof(healthReport));
        }

        context.Response.ContentType = "application/json; charset=utf-8";

        var options = new JsonWriterOptions { Indented = true };

        using var memoryStream = new MemoryStream();
        using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("status", healthReport.Status.ToString());
            jsonWriter.WriteStartObject("results");

            foreach (var healthReportEntry in healthReport.Entries)
            {
                jsonWriter.WriteStartObject(healthReportEntry.Key);
                jsonWriter.WriteString(
                    "status",
                    healthReportEntry.Value.Status.ToString());
                jsonWriter.WriteString(
                    "description",
                    healthReportEntry.Value.Description);
                jsonWriter.WriteStartObject("data");

                foreach (var item in healthReportEntry.Value.Data)
                {
                    jsonWriter.WritePropertyName(item.Key);

                    JsonSerializer.Serialize(
                        jsonWriter,
                        item.Value,
                        item.Value?.GetType() ?? typeof(object));
                }

                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }

            jsonWriter.WriteEndObject();
            jsonWriter.WriteEndObject();
        }

        return context.Response.WriteAsync(
            Encoding.UTF8.GetString(memoryStream.ToArray()));
    }
}
