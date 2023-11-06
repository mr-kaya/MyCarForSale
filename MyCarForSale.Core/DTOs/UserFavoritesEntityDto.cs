namespace MyCarForSale.Core.DTOs;

public class UserFavoritesEntityDto
{
    public int Id { get; set; }
    public int FavoriteUserId { get; set; }
    public int FavoriteBaseId { get; set; }
}