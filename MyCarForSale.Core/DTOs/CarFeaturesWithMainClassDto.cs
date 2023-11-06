namespace MyCarForSale.Core.DTOs;

public class CarFeaturesWithMainClassDto : CarFeaturesEntityDto
{
    public MainClassificationEntityDto MainClassificationEntity { get; set; }
}