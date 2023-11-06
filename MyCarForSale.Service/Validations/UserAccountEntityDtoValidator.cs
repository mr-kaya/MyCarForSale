using FluentValidation;
using MyCarForSale.Core.DTOs;

namespace MyCarForSale.Service.Validations;

public class UserAccountEntityDtoValidator : AbstractValidator<UserAccountEntityDto>
{
    public UserAccountEntityDtoValidator()
    {
        RuleFor(x => x.Email).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").EmailAddress()
            .WithMessage("Lütfen geçerli bir e-posta adresi giriniz.");
        RuleFor(x => x.Name).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").MaximumLength(50)
            .WithMessage("İsminiz 50 karakteri geçmemelidir.");
        RuleFor(x => x.Surname).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").MaximumLength(50)
            .WithMessage("Soyadınız 50 karakteri geçmemelidir.");
        RuleFor(x => x.Birthday).Must(birthday => (DateTime.Now.Year - birthday.Year) >= 18)
            .WithMessage("Üye olmak için 18 yaşından büyük olmalısınız.")
            .Must(birthday => (DateTime.Now.Year - birthday.Year) <= 100)
            .WithMessage("Lütfen gerçek doğum tarihinizi giriniz.");
        RuleFor(x => x.PhoneNumber).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").Must(x => x.Length > 9 && x.Length < 12)
            .WithMessage("Gerçek bir telefon numarası giriniz.").Must(x => int.TryParse(x, out _))
            .WithMessage("Lütfen ülke kodu (+) kullanmayın.");
        RuleFor(x => x.Country).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.");
        RuleFor(x => x.Province).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.");
        RuleFor(x => x.District).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.");
        RuleFor(x => x.FullAddress).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").MaximumLength(254)
            .WithMessage("Karakter sınırını aştınız. Lütfen, en fazla 254 karakter kullanın.");

        RuleFor(x => x.ZipCode).NotNull().WithMessage("{PropertyName} alanı boş bırakılamaz.").NotEmpty()
            .WithMessage("{PropertyName} alanı boş bırakılamaz.").Must(x => int.TryParse(x.ToString(), out _))
            .WithMessage("Lütfen sadece rakam giriniz.").Must(x => x.ToString().Length < 12)
            .WithMessage("Gerçek bir posta kodu giriniz.");
    }
    
}