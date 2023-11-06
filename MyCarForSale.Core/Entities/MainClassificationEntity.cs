namespace MyCarForSale.Core.Entities;

public class MainClassificationEntity
{
    public int Id { get; set; }
    public string MainClassification { get; set; }
    public string CarBrand { get; set; }
    public string CarModel { get; set; }
    public string CarPackage { get; set; }
    public ushort CarYear { get; set; }
}