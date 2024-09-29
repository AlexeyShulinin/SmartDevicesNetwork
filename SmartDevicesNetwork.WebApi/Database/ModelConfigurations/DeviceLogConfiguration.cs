using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartDevicesNetwork.WebApi.Database.Models;

namespace SmartDevicesNetwork.WebApi.Database.ModelConfigurations;

public class DeviceLogConfiguration : IEntityTypeConfiguration<DeviceLog>
{
    public void Configure(EntityTypeBuilder<DeviceLog> builder)
    {
        builder.HasKey(x => x.LogId);
        builder.Property(x => x.LogId).UseIdentityColumn(1);
        
        builder.Property(x => x.TimeStamp).HasColumnType("datetimeoffset");
        
        builder.Property(x => x.Message).HasMaxLength(255).IsUnicode();
        
        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}