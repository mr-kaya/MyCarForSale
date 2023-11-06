using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Repository.Seeds;

public class CarFeaturesEntitySeed : IEntityTypeConfiguration<CarFeaturesEntity>
{
    public void Configure(EntityTypeBuilder<CarFeaturesEntity> builder)
    {
        builder.HasData(
            new CarFeaturesEntity()
            {
                Id = 1, MainClassificationEntityId = 1, AdvertisementName = "Garaj Arabası", AdvertisementDescription = "Daha 22.000km'de fazla söze gerek yok.", /*CreateDate = DateTime.Now,*/ EngineHorsePower = 115, EngineTorque = 125, EngineDisplacement = 1.6F, EngineFuelType = "Diesel", TransmissionType = "Automatic", CarTotalKm = 22300, CarGuarantee = true, CarDrivetrain = "Front", PublishUserId = 1
            },
            new CarFeaturesEntity()
            {
                Id = 2, MainClassificationEntityId = 2, AdvertisementName = "Acil Satılık", AdvertisementDescription = "İhtiyaç dolayısıyla satılıktır.", /*CreateDate = DateTime.Now,*/ EngineHorsePower = 220, EngineTorque = 250, EngineDisplacement = 2.0F, EngineFuelType = "Gasoline", TransmissionType = "Manuel", CarTotalKm = 120000, CarGuarantee = false, CarDrivetrain = "Back", PublishUserId = 2
            },
            new CarFeaturesEntity()
            {
                Id = 3, MainClassificationEntityId = 3, AdvertisementName = "Öğretmenden Satılık", AdvertisementDescription = "Hatasızdır.", /*CreateDate = DateTime.Now,*/ EngineHorsePower = 69, EngineTorque = 135, EngineDisplacement = 1.4F, EngineFuelType = "Diesel", TransmissionType = "Manuel", CarTotalKm = 200000, CarGuarantee = false, CarDrivetrain = "Back", PublishUserId = 2
            }
        );
    }
}