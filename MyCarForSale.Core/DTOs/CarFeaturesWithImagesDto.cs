namespace MyCarForSale.Core.DTOs;

public class CarFeaturesWithImagesDto : CarFeaturesEntityDto
{
    public ICollection<CarImagesEntityDto> CarImagesEntities { get; set; }
}