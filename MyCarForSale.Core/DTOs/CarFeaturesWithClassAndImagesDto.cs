namespace MyCarForSale.Core.DTOs;

public class CarFeaturesWithClassAndImagesDto : CarFeaturesEntityDto
{
    public MainClassificationEntityDto MainClassificationEntity { get; set; }
    
    public ICollection<CarImagesEntityDto> CarImagesEntities { get; set; }

    public string UserAccountEntityName { get; set; }
    public string UserAccountEntitySurname { get; set; }
    public string UserAccountEntityPhoneNumber { get; set; }
}