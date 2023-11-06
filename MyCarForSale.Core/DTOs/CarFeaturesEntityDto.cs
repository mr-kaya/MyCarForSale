namespace MyCarForSale.Core.DTOs;

public class CarFeaturesEntityDto : BaseEntityDto
{
    public ushort EngineHorsePower { get; set; }
    public ushort EngineTorque { get; set; }
    public int MainClassificationEntityId { get; set; }
    public int PublishUserId { get; set; }
    public float EngineDisplacement { get; set; }
    public string EngineFuelType { get; set; }
    public string TransmissionType { get; set; }
    public int CarTotalKm { get; set; }
    public string CarDrivetrain { get; set; }
    public bool CarGuarantee { get; set; }
}