using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SmartDevicesNetwork.WebApi.Database.Models;
using Action = SmartDevicesNetwork.WebApi.Database.Models.Action;

namespace SmartDevicesNetwork.WebApi.Database;

public class SdnDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Action> Actions { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceLog> DeviceLogs { get; set; }
    public DbSet<NetworkLink> NetworkLinks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}