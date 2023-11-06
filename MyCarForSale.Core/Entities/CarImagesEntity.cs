using System.ComponentModel.DataAnnotations.Schema;

namespace MyCarForSale.Core.Entities;

public class CarImagesEntity
{
    
    public int Id { get; set; }
    public int BaseEntityId { get; set; }
    public BaseEntity BaseEntityCarImageEntity { get; set; } = null!;
    public string CarImageUrl { get; set; }
}