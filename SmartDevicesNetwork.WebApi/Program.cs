using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SmartDevicesNetwork.WebApi;
using SmartDevicesNetwork.WebApi.BackgroundServices;
using SmartDevicesNetwork.WebApi.Database;
using SmartDevicesNetwork.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<SdnDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SmartDevicesNetwork")));

builder.Services.AddMemoryCache();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddHostedService<NetworkEmulator>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US")
    };
    options.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "localhost",
        policy  =>
        {
            policy.WithOrigins("http://localhost:5173");
        });
});

var app = builder.Build();

app.RegisterEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionsHandlingMiddleware>();

var webSocketOptions = new WebSocketOptions
{
    AllowedOrigins = { "http://localhost:5173" }
};

app.UseWebSockets(webSocketOptions);
app.UseCors("localhost");

app.Run();
