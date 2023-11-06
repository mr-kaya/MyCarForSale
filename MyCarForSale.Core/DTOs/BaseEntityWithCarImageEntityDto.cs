namespace MyCarForSale.Core.DTOs;

public class BaseEntityWithCarImageEntityDto : BaseEntityDto
{
    public CarImagesEntityDto CarImages { get; set; }
}