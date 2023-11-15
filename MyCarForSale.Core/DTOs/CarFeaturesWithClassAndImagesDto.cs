namespace MyCarForSale.Core.DTOs;

public class CarFeaturesWithClassAndImagesDto : CarFeaturesEntityDto
{
    public MainClassificationEntityDto MainClassificationEntity { get; set; }
    
    public ICollection<CarImagesEntityDto> CarImagesEntities { get; set; }

    public UserAccountEntityDto UserAccountEntity{ get; set; }
}