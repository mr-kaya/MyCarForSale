namespace MyCarForSale.Core.DTOs;

public class CarFeaturesWithImageAndClassificationDto : CarFeaturesEntityDto
{
    public ICollection<CarImagesEntityDto> CarImagesEntities { get; set; }
    public MainClassificationEntityDto MainClassificationEntity { get; set; }
}