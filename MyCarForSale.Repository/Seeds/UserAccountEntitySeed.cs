using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Repository.Seeds;

public class UserAccountEntitySeed : IEntityTypeConfiguration<UserAccountEntity>
{
    public void Configure(EntityTypeBuilder<UserAccountEntity> builder)
    {
        builder.HasData(
            new UserAccountEntity()
            {
                Id = 1, Name = "Mustafa", Surname = "Kaya", Email = "kaya.mustafa.467@gmail.com", Password = "baron099", PhoneNumber = "+905354443322", Country = "Turkey", Province = "Aydin", District = "Germencik", FullAddress = "Camikebir Mh. Konak Sk. No:7 D:7 Kaya Apt.", ZipCode = "09700", Authorization = "Root"
            },
            new UserAccountEntity()
            {
                Id = 2, Name = "Yiğitcan", Surname = "Kaya", Email = "yigitcan.doktor.0960@hotmail.com", Password = "yigit123", PhoneNumber = "+905359156644", Country = "Turkey", Province = "Bursa", District = "Nilüfer", FullAddress = "Doğa Mh. Can Sk. No:12 D:23", ZipCode = "16315", Authorization = "User"
            });
    }
}