using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartDevicesNetwork.WebApi.Database.Models;

namespace SmartDevicesNetwork.WebApi.Database.ModelConfigurations;

public class NetworkLinkConfiguration : IEntityTypeConfiguration<NetworkLink>
{
    public void Configure(EntityTypeBuilder<NetworkLink> builder)
    {
        builder.HasKey(x => x.LinkId);
        builder.Property(x => x.LinkId).UseIdentityColumn(1);
        
        builder.Property(x => x.LinkType).HasMaxLength(50).IsUnicode();
        
        builder.HasOne(x => x.SourceDevice)
            .WithMany()
            .HasForeignKey(x => x.SourceId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(x => x.TargetDevice)
            .WithMany()
            .HasForeignKey(x => x.TargetId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}