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
    public string UserAccountEntityId { get; set; }
    public string UserAccountEntityEmail { get; set; }
    public string UserAccountEntityName { get; set; }
    public string UserAccountEntitySurname { get; set; }
    public string UserAccountEntityPhoneNumber{ get; set; }
    public string UserAccountEntityCountry { get; set; }
    public string UserAccountEntityProvince { get; set; }
    public string UserAccountEntityDistrict { get; set; }
    public string UserAccountEntityFullAddress { get; set; }
    public string UserAccountEntityZipCode { get; set; }
}