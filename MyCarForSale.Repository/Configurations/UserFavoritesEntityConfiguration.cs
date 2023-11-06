using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCarForSale.Core.Entities;

namespace MyCarForSale.Repository.Configurations;

public class UserFavoritesEntityConfiguration : IEntityTypeConfiguration<UserFavoritesEntity>
{
    public void Configure(EntityTypeBuilder<UserFavoritesEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.HasOne(x => x.UserAccountEntity).WithMany().HasForeignKey(x => x.FavoriteUserId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.BaseEntity).WithMany().HasForeignKey(x => x.FavoriteBaseId).OnDelete(DeleteBehavior.NoAction);
    }
}