using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartDevicesNetwork.WebApi.Database.Models;

namespace SmartDevicesNetwork.WebApi.Database.ModelConfigurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(x => x.DeviceId);
        builder.Property(x => x.DeviceId).UseIdentityColumn(1);
        
        builder.Property(x => x.Name).HasMaxLength(100).IsUnicode();
        builder.Property(x => x.Type).HasMaxLength(50).IsUnicode();
        builder.Property(x => x.Status).HasMaxLength(20).IsUnicode();
        
        builder.Property(x => x.LastActive).HasColumnType("datetime");
        
        builder.Property(x => x.IpAddress).HasMaxLength(50).IsUnicode();
        builder.Property(x => x.FirmwareVersion).HasMaxLength(20).IsUnicode();
    }
}