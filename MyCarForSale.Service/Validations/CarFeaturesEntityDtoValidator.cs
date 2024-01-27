using System.Globalization;
using FluentValidation;
using MyCarForSale.Core.DTOs;

namespace MyCarForSale.Service.Validations;

public class CarFeaturesEntityDtoValidator : AbstractValidator<CarFeaturesEntityDto>
{
    public CarFeaturesEntityDtoValidator()
    {
        RuleFor(x => x.AdvertisementName).NotNull().WithMessage("{PropertyName} alanını boş bırakılamaz.")
            .NotEmpty().WithMessage("{PropertyName} alanının doldurulması zorunludur.").MaximumLength(50).WithMessage("En fazla 50 karakter kullanabilirsiniz.");
        RuleFor(x => x.AdvertisementDescription).NotNull().WithMessage("{PropertyName} alanını boş bırakılamaz.")
            .NotEmpty().WithMessage("{PropertyName} alanı boş bırakılamaz.").MaximumLength(500).WithMessage("En fazla 500 karakter kullanabilirsiniz.");
        RuleFor(x => x.Price).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").GreaterThan(0)
            .WithMessage("Girdiğiniz değer '0'dan büyük olmalıdır.");
        
        List<string> drivetrainList = new List<string>() {"Front","Back","FourMotion"};
        RuleFor(x => x.CarDrivetrain).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.")
            .Must(x => drivetrainList.Contains(x.ToString()))
            .WithMessage("Girilen değer, listede bulunamadı!");
        
        RuleFor(x => x.EngineHorsePower).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.");
        RuleFor(x => x.EngineTorque).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.");
        RuleFor(x => x.MainClassificationEntityId).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.")
            .NotEmpty().WithMessage("{PropertyName} alanı boş bırakılamaz.");
        RuleFor(x => x.PublishUserId).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.");
        RuleFor(x => x.EngineDisplacement).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").Must(x => float.TryParse(x.ToString(CultureInfo.CurrentCulture), out _))
            .WithMessage("Girilen ifade bir sayı olmalıdır.");

        List<string> fuelTypeList = new List<string>() { "Gasoline", "Diesel", "LPG", "Electric", "Hybrid", "Hydrogen" };
        RuleFor(x => x.EngineFuelType).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").Must(x => fuelTypeList.Contains(x.ToString()))
            .WithMessage("Girilen değer, listede bulunamadı!");
        
        List<string> transmissionList = new List<string>() { "Manuel", "Automatic", "Semi-Automatic" };
        RuleFor(x => x.TransmissionType).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.")
            .Must(x => transmissionList.Contains(x.ToString()))
            .WithMessage("Girilen değer, listede bulunamadı!");
        RuleFor(x => x.CarTotalKm).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").Must(x => int.TryParse(x.ToString(), out _))
            .WithMessage("Girilen ifade bir sayı olamalıdır.");
    }
}