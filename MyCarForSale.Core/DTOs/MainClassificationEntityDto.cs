namespace MyCarForSale.Core.DTOs;

public class MainClassificationEntityDto
{
    public int Id { get; set; }
    public string MainClassification { get; set; }
    public string CarBrand { get; set; }
    public string CarModel { get; set; }
    public string CarPackage { get; set; }
    public ushort CarYear { get; set; }
}