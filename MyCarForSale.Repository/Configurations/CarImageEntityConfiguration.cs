using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Repository.Configurations;

public class CarImageEntityConfiguration : IEntityTypeConfiguration<CarImagesEntity>
{
    public void Configure(EntityTypeBuilder<CarImagesEntity> builder)
    {
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        
        builder.HasOne(x => x.BaseEntityCarImageEntity).WithMany(x => x.CarImagesEntities)
            .HasForeignKey(x => x.BaseEntityId);
    }
}