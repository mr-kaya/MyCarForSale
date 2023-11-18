namespace MyCarForSale.Core.DTOs;

public class BaseEntityDto
{
    public int Id { get; set; }
    public string AdvertisementName { get; set; }
    public string AdvertisementDescription { get; set; }
    public int Price { get; set; }
}