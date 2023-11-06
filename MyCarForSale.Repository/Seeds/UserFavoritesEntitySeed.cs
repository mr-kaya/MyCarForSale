using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Repository.Seeds;

public class UserFavoritesEntitySeed : IEntityTypeConfiguration<UserFavoritesEntity>
{
    public void Configure(EntityTypeBuilder<UserFavoritesEntity> builder)
    {
        builder.HasData(
            new UserFavoritesEntity()
            {
                Id = 1, FavoriteUserId = 1, FavoriteBaseId = 3
            },
            new UserFavoritesEntity()
            {
                Id = 2, FavoriteUserId = 2, FavoriteBaseId = 1
            },
            new UserFavoritesEntity()
            {
                Id = 3, FavoriteUserId = 2, FavoriteBaseId = 2
            });
    }
}