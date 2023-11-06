using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Repository.Seeds;

public class MainClassificationEntitySeed : IEntityTypeConfiguration<MainClassificationEntity>
{
    public void Configure(EntityTypeBuilder<MainClassificationEntity> builder)
    {
        builder.HasData(
            new MainClassificationEntity()
            {
                Id = 1, MainClassification = "Automobile", CarBrand = "Ford", CarModel = "Focus", CarPackage = "Titanium X", CarYear = 2012
            },
            new MainClassificationEntity()
            {
                Id = 2, MainClassification = "Automobile", CarBrand = "BMW", CarModel = "520i", CarPackage = "Luxury Line", CarYear = 2016
            },
            new MainClassificationEntity()
            {
                Id = 3, MainClassification = "Automobile", CarBrand = "Fiat", CarModel = "Punto", CarPackage = "Evo", CarYear = 2011
            }
        );
    }
}