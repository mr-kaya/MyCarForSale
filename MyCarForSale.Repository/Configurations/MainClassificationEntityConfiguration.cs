using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Repository.Configurations;

public class MainClassificationEntityConfiguration : IEntityTypeConfiguration<MainClassificationEntity>
{
    public void Configure(EntityTypeBuilder<MainClassificationEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.MainClassification).IsRequired().HasMaxLength(70);
        builder.Property(x => x.CarBrand).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CarModel).IsRequired().HasMaxLength(80);
        builder.Property(x => x.CarPackage).IsRequired().HasMaxLength(150);
        builder.Property(x => x.CarYear).IsRequired().HasMaxLength(4);
    }
}