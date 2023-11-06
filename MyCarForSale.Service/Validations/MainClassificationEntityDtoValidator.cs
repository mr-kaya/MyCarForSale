using FluentValidation;
using MyCarForSale.Core.DTOs;

namespace MyCarForSale.Service.Validations;

public class MainClassificationEntityDtoValidator : AbstractValidator<MainClassificationEntityDto>
{
    public MainClassificationEntityDtoValidator()
    {
        RuleFor(x => x.MainClassification).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").MaximumLength(70).WithMessage("Girilen değer en fazla 70 karakter olabilir.");
        RuleFor(x => x.CarBrand).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").MaximumLength(50).WithMessage("Girilen değer en fazla 50 karakter olabilir.");
        RuleFor(x => x.CarModel).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").MaximumLength(80).WithMessage("Girilen değer en fazla 80 karakter olabilir.");
        RuleFor(x => x.CarPackage).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").MaximumLength(150).WithMessage("Girilen değer en fazla 150 karakter olabilir.");
        RuleFor(x => x.CarYear).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").Must(x => ushort.TryParse(x.ToString(), out _))
            .WithMessage("Lütfen sadece rakam giriniz.").Must(x=> x.ToString().Length == 4).WithMessage("Girilen değer sadece yıl olmalıdır.");
    }
}