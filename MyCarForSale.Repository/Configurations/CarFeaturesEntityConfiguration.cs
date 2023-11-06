using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Repository.Configurations;

public class CarFeaturesEntityConfiguration : IEntityTypeConfiguration<CarFeaturesEntity>
{
    public void Configure(EntityTypeBuilder<CarFeaturesEntity> builder)
    {
        builder.Property(x => x.CarTotalKm).IsRequired().HasMaxLength(10);
        builder.Property(x => x.CarDrivetrain).IsRequired();
        builder.Property(x => x.MainClassificationEntityId).IsRequired();
        builder.Property(x => x.TransmissionType).IsRequired();
        builder.Property(x => x.EngineFuelType).IsRequired();
        builder.Property(x => x.EngineDisplacement).IsRequired();

        builder.Property(x => x.AdvertisementName).IsRequired();
        builder.Property(x => x.AdvertisementDescription).IsRequired();

        builder.HasOne(x => x.UserAccountEntity).WithMany().HasForeignKey(x => x.PublishUserId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.MainClassificationEntity).WithMany().HasForeignKey(x => x.MainClassificationEntityId).OnDelete(DeleteBehavior.NoAction);
    }
}