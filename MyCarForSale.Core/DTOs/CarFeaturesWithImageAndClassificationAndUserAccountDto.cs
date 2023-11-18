namespace MyCarForSale.Core.DTOs;

public class CarFeaturesWithImageAndClassificationAndUserAccountDto : CarFeaturesEntityDto
{
    public MainClassificationEntityDto MainClassificationEntity { get; set; }
    public string MainClassificationEntityMainClassification { get; set; }
    public string MainClassificationEntityCarBrand { get; set; }
    public string MainClassificationEntityCarModel { get; set; }
    public string MainClassificationEntityCarPackage { get; set; }
    public string MainClassificationEntityCarYear { get; set; }

    public ICollection<CarImagesEntityDto> CarImagesEntities { get; set; }
    public ICollection<string> CarImagesEntitiesCarImageUrl { get; set; }
    
    public UserAccountEntityDto UserAccountEntity { get; set; }
}