using System.ComponentModel.DataAnnotations;

namespace MyCarForSale.Core.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public int PublishUserId { get; set; }
    public UserAccountEntity? UserAccountEntity { get; set; }
    public string AdvertisementName { get; set; }
    public string AdvertisementDescription { get; set; }
    public int Price { get; set; }

    public ICollection<CarImagesEntity>? CarImagesEntities { get; set; }
    public int MainClassificationEntityId { get; set; }
    public MainClassificationEntity? MainClassificationEntity { get; set; }
    /*
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    */  
}