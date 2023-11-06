using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Repository.Seeds;

public class CarImageEntitySeed : IEntityTypeConfiguration<CarImagesEntity>
{
    public void Configure(EntityTypeBuilder<CarImagesEntity> builder)
    {
        builder.HasData(
            new CarImagesEntity()
            {
                Id = 1, BaseEntityId = 1, CarImageUrl = "http://www.arabalar.com.tr/static-res/image/versiyon/660x495/ford/focus/sedan/2012/2012-ford-focus-sedan-03.jpg"
            },
            new CarImagesEntity()
            {
                Id = 2, BaseEntityId = 1, CarImageUrl = "http://www.arabalar.com.tr/static-res/image/versiyon/660x495/ford/focus/sedan/2012/2012-ford-focus-sedan-01.jpg"
            },
            new CarImagesEntity()
            {
                Id = 3, BaseEntityId = 1, CarImageUrl = "http://www.arabalar.com.tr/static-res/image/versiyon/660x495/ford/focus/sedan/2012/2012-ford-focus-sedan-08.jpg"
            },
            new CarImagesEntity()
            {
                Id = 4, BaseEntityId = 2, CarImageUrl = "http://www.arabalar.com.tr/static-res/image/versiyon/660x495/bmw/5-serisi/sedan/2012/2012-bmw-5-serisi-sedan-15.jpg"
            },
            new CarImagesEntity()
            {
                Id = 5, BaseEntityId = 2, CarImageUrl = "http://www.arabalar.com.tr/static-res/image/versiyon/660x495/bmw/5-serisi/sedan/2012/2012-bmw-5-serisi-sedan-22.jpg"
            },
            new CarImagesEntity()
            {
                Id = 6, BaseEntityId = 2, CarImageUrl = "http://www.arabalar.com.tr/static-res/image/versiyon/660x495/bmw/5-serisi/sedan/2012/2012-bmw-5-serisi-sedan-02.jpg"
            },
            new CarImagesEntity()
            {
                Id = 7, BaseEntityId = 2, CarImageUrl = "http://www.arabalar.com.tr/static-res/image/versiyon/660x495/bmw/5-serisi/sedan/2012/2012-bmw-5-serisi-sedan-111.jpg"
            },
            new CarImagesEntity()
            {
                Id = 8, BaseEntityId = 3, CarImageUrl = "http://www.arabalar.com.tr/static-res/image/versiyon/660x495/fiat/punto/hb/2011/2011-fiat-punto-hb-09.jpg"
            },
            new CarImagesEntity()
            {
                Id = 9, BaseEntityId = 3, CarImageUrl = "http://www.arabalar.com.tr/static-res/image/versiyon/660x495/fiat/punto/hb/2011/2011-fiat-punto-hb-05.jpg"
            },
            new CarImagesEntity()
            {
                Id = 10, BaseEntityId = 3, CarImageUrl = "http://www.arabalar.com.tr/static-res/image/versiyon/660x495/fiat/punto/hb/2011/2011-fiat-punto-hb-11.jpg"
            },
            new CarImagesEntity()
            {
                Id = 11, BaseEntityId = 3, CarImageUrl = "http://www.arabalar.com.tr/static-res/image/versiyon/660x495/fiat/punto/hb/2011/2011-fiat-punto-hb-07.jpg"
            },
            new CarImagesEntity()
            {
                Id = 12, BaseEntityId = 3, CarImageUrl = "http://www.arabalar.com.tr/static-res/image/versiyon/660x495/fiat/punto/hb/2011/2011-fiat-punto-hb-03.jpg"
            }
        );
    }
}