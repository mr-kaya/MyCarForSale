namespace MyCarForSale.Core.Entities;

public class UserFavoritesEntity
{
    
    public int Id { get; set; }
    
    public int FavoriteUserId { get; set; }
    public UserAccountEntity? UserAccountEntity { get; set; }
    public int FavoriteBaseId { get; set; }
    public BaseEntity? BaseEntity { get; set; }
}