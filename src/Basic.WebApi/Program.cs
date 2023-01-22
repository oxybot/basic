// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.DataAccess;
using Basic.WebApi.Framework;
using Basic.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add entity framework services to the container
builder.Services.AddDbContext<Context>(options => DbContextInitializer.InitializeOptions(options, builder.Configuration));

// Add custom options for the project
builder.Services.Configure<ActiveDirectoryOptions>(builder.Configuration.GetSection(ActiveDirectoryOptions.Section));

// Add a configuration source based on database
builder.Configuration.AddDatabase(builder.Logging);

// Enforce lowercase controller names
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Enable string representation for enums
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // Enable support of DateOnly
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    })
    .AddMvcOptions(options =>
    {
        // Default display name management for fields
        // used for automated error messages
        options.ModelMetadataDetailsProviders.Add(new HumanizerMetadataProvider());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            return new InvalidModelStateActionResult(context.ModelState);
        };
    });

builder.Services.AddHttpContextAccessor();

// Enable healthcheck
builder.Services.AddHealthChecks()
    .AddDbContextCheck<Context>(name: "Datasource");

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            var origins = builder.Configuration["cors:origins"]
                .Split(",")
                .Select(i => i.Trim())
                .ToArray();
            policy
                .WithOrigins(origins)
                .WithHeaders("content-type", "authorization")
                .WithMethods("GET", "POST", "PUT", "DELETE");
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = false;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = new TimeSpan(0, 0, 10),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["BaseUrl"],
            ValidIssuer = builder.Configuration["BaseUrl"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtToken:SecretKey"])),
        };
    });

// Enforce the validation of the jwt token based on the approved token
builder.Services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, CustomJwtBearerPostConfigure>();

// Define the content of the openapi definition files
builder.Services.AddTransient<IApiDescriptionProvider, DefaultSuccessResponseProvider>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Use xml comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    // Add support for DateOnly
    options.MapType<DateOnly>(() => new OpenApiSchema() { Type = "string", Format = "date" });

    // Override xml comments with annotations
    options.EnableAnnotations();

    // Add security configuration
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter \"Bearer\" [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });

    options.OperationFilter<RoleRequirementsOperationFilter>();
    options.OperationFilter<OperationIdFilter>();

    options.AddServer(new OpenApiServer() { Url = builder.Configuration["BaseUrl"] });
    var license = new OpenApiLicense()
    {
        Name = "MIT",
        Url = new Uri("https://github.com/oxybot/basic/blob/main/LICENSE"),
    };
    var contact = new OpenApiContact()
    {
        Name = "oxybot",
        Url = new Uri("https://github.com/oxybot/basic"),
    };
    var info = new OpenApiInfo()
    {
        Title = "Basic API",
        Version = "1.0",
        License = license,
        Description = Assembly.GetExecutingAssembly().ReadResource("Basic.WebApi.README.md"),
        Contact = contact,
    };

    options.SwaggerDoc("basic", info);
});

// Business services and options
builder.Services
    .AddOptions<EmailServiceOptions>()
    .BindConfiguration(EmailServiceOptions.Section);
builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsSnapshot<EmailServiceOptions>>().Value);

builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<EventRequestService>();
builder.Services.AddScoped<ConsumptionService>();
builder.Services.AddScoped<Context>();
builder.Services.AddSingleton<ExternalAuthenticatorService>();
builder.Services.AddScoped<DefinitionsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

// Configure the healthcheck response as a json format
app.MapHealthChecks("/healthchecks", new HealthCheckOptions()
{
    ResponseWriter = HealthCheck.ResponseWriterAsync,
});

// Generate the basic-openapi.json/yaml files
app.UseSwagger(options =>
{
    options.RouteTemplate = "/{documentName}-openapi.{json|yaml}";
});

// Enable swagger-ui
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/basic-openapi.json", "Basic API V1");
    options.RoutePrefix = string.Empty;
});

// Enable redoc ui
app.UseReDoc(options =>
{
    options.SpecUrl = "/basic-openapi.json";
    options.RoutePrefix = "redoc";
});

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseException();

// Apply migrations if needed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Context>();
    db.Database.Migrate();
}

app.Run();
