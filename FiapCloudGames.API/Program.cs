using FiapCloudGames.Application;
using FiapCloudGames.Infrastructure;
using FiapCloudGames.Infrastructure.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddUserSecrets<Program>(optional: true);

var newRelicKey = builder.Configuration["NEW_RELIC_LICENSE_KEY"];

var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console();

if (!string.IsNullOrWhiteSpace(newRelicKey))
{
    loggerConfig = loggerConfig.WriteTo.NewRelicLogs(
        licenseKey: newRelicKey,
        applicationName: "FiapCloudGames"
    );
}

Log.Logger = loggerConfig.CreateLogger();

builder.Host.UseSerilog();


builder.Services.AddApplication();
builder.Services.AddInfrastructureModule(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FiapCloudGames API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Description = "Please enter your Bearer token in the format **'Bearer {your token here}'**"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCorrelationMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
