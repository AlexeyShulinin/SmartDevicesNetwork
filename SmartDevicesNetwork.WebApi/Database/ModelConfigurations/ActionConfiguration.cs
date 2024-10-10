using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Action = SmartDevicesNetwork.WebApi.Database.Models.Action;
using Models_Action = SmartDevicesNetwork.WebApi.Database.Models.Action;

namespace SmartDevicesNetwork.WebApi.Database.ModelConfigurations;

public class ActionConfiguration : IEntityTypeConfiguration<Models_Action>
{
    public void Configure(EntityTypeBuilder<Action> builder)
    {
        builder.HasKey(x => x.ActionId);
        builder.Property(x => x.ActionId).UseIdentityColumn(1);

        builder.Property(x => x.ActionType).HasMaxLength(50).IsUnicode();

        builder.Property(x => x.TimeStamp).HasColumnType("datetimeoffset");

        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}