using System.ComponentModel.DataAnnotations;

namespace MyCarForSale.Core.Entities;

public class CarFeaturesEntity:BaseEntity
{
    public ushort? EngineHorsePower { get; set; }
    public ushort? EngineTorque { get; set; }
    public float EngineDisplacement { get; set; }
    public string EngineFuelType { get; set; }
    public string TransmissionType { get; set; }
    public int CarTotalKm { get; set; }
    public string CarDrivetrain { get; set; }
    public bool? CarGuarantee { get; set; }
}