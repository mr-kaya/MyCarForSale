using FluentValidation;
using MyCarForSale.Core.DTOs;

namespace MyCarForSale.Service.Validations;

public class UserAccountEntityDtoValidator : AbstractValidator<UserAccountEntityDto>
{
    public UserAccountEntityDtoValidator()
    {
        RuleFor(x => x.Authorization).NotNull().NotEmpty();
        RuleFor(x => x.Email).NotNull().WithMessage("Enter your {PropertyName}.").NotEmpty()
            .WithMessage("Enter your {PropertyName}.").EmailAddress().NotEmpty().WithMessage("Enter your {PropertyName}.").NotNull().WithMessage("Enter your {PropertyName}.")
            .WithMessage("Please enter a valid e-mail address.");
        RuleFor(x => x.Password).NotNull().WithMessage("Enter your {PropertyName}.").NotEmpty()
            .WithMessage("Enter your {PropertyName}.").MinimumLength(6)
            .WithMessage("Minimum 6 characters required").MaximumLength(Int32.MaxValue).WithMessage("You have reached the character limit. Congratulations");
        RuleFor(x => x.Name).NotNull().WithMessage("Enter your {PropertyName}.").NotEmpty()
            .WithMessage("Enter your {PropertyName}.").MaximumLength(50)
            .WithMessage("Maximum 50 characters");
        RuleFor(x => x.Surname).NotNull().WithMessage("Enter your {PropertyName}.").NotEmpty()
            .WithMessage("Enter your {PropertyName}.").MaximumLength(50)
            .WithMessage("Maximum 50 characters");
        RuleFor(x => x.Birthday).Must(birthday => (DateTime.Now.Year - birthday.Year) >= 18)
            .WithMessage("You must be over 18 to register.")
            .Must(birthday => (DateTime.Now.Year - birthday.Year) <= 100)
            .WithMessage("Please enter your actual date of birth.");
        RuleFor(x => x.PhoneNumber).NotNull().WithMessage("Enter your {PropertyName}.").NotEmpty()
            .WithMessage("Enter your {PropertyName}.").MinimumLength(10)
            .WithMessage("Enter a real phone number.").MaximumLength(11)
            .WithMessage("Enter a real phone number.").Must(x => double.TryParse(x, out _))
            .WithMessage("Only number and (+)");
        RuleFor(x => x.Country).NotNull().WithMessage("Enter your {PropertyName}.").NotEmpty()
            .WithMessage("Enter your {PropertyName}.");
        RuleFor(x => x.Province).NotNull().WithMessage("Enter your {PropertyName}.").NotEmpty()
            .WithMessage("Enter your {PropertyName}.");
        RuleFor(x => x.District).NotNull().WithMessage("Enter your {PropertyName}.").NotEmpty()
            .WithMessage("Enter your {PropertyName}.");
        RuleFor(x => x.FullAddress).NotNull().WithMessage("Enter your {PropertyName}.").NotEmpty()
            .WithMessage("Enter your {PropertyName}.").MaximumLength(254)
            .WithMessage("Maximum 254 characters");

        RuleFor(x => x.ZipCode).NotNull().WithMessage("Enter your {PropertyName}").NotEmpty()
            .WithMessage("Enter your {PropertyName}").Must(x => int.TryParse(x.ToString(), out _))
            .WithMessage("Please enter only numbers").Must(x => x.ToString().Length < 12)
            .WithMessage("Enter a real zip code.");
    }
    
}