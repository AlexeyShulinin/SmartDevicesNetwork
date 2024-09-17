using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SmartDevicesNetwork.WebApi;
using SmartDevicesNetwork.WebApi.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<SdnDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SmartDevicesNetwork")));

builder.Services.AddMemoryCache();
builder.Services.AddRepositories();
builder.Services.AddServices();
//builder.Services.AddHostedService<NetworkEmulator>();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.RegisterEndpoints();
app.RegisterWebSocketsHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionsHandlingMiddleware>();
app.UseWebSockets();

app.Run();
