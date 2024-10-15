using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SmartDevicesNetwork.WebApi;
using SmartDevicesNetwork.WebApi.BackgroundServices;
using SmartDevicesNetwork.WebApi.Database;
using SmartDevicesNetwork.WebApi.Endpoints;
using SmartDevicesNetwork.WebApi.Options;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;
using WebSocketOptions = Microsoft.AspNetCore.Builder.WebSocketOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

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

var cultureOptions = builder.Configuration.GetSection("Cultures").Get<CultureOptions>();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = cultureOptions.SupportedCultures.Select(x => new CultureInfo(x)).ToList();
    options.DefaultRequestCulture = new RequestCulture(cultureOptions.DefaultCulture, cultureOptions.DefaultCulture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var corsOptions = builder.Configuration.GetSection("Cors").Get<CorsOptions>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsOptions.Name,
        policy  =>
        {
            policy.WithOrigins(corsOptions.AllowedOrigins).AllowAnyMethod().AllowAnyHeader();
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
    AllowedOrigins = { corsOptions.AllowedOrigins }
};


app.UseWebSockets(webSocketOptions);
app.UseCors(corsOptions.Name);

app.Run();
