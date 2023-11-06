using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Repository.Configurations;

public class UserAccountEntityConfiguration : IEntityTypeConfiguration<UserAccountEntity>
{
    public void Configure(EntityTypeBuilder<UserAccountEntity> builder)
    {
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Surname).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.PhoneNumber).IsRequired();
        builder.Property(x => x.Country).IsRequired();
        builder.Property(x => x.Province).IsRequired();
        builder.Property(x => x.District).IsRequired();
        builder.Property(x => x.FullAddress).HasMaxLength(254);
        builder.Property(x => x.ZipCode).IsRequired().HasMaxLength(12);
    }
}