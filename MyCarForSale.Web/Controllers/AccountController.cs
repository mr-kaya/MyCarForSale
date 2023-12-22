using System.Globalization;
using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Web.Services;

namespace MyCarForSale.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserAccountService _accountService;
    private readonly IValidator<UserAccountEntityDto> _validator;

    public AccountController(UserAccountService accountService, IValidator<UserAccountEntityDto> validator)
    {
        _accountService = accountService;
        _validator = validator;
    }

    public async Task<IActionResult> AccountDetailsPage()
    {
        var userAccountDto = _accountService.GetUserById();
        return View(await userAccountDto);
    }

    public async Task<IActionResult> AccountPasswordPage()
    {
        var userAccuntDto = _accountService.GetUserById();
        return View(await userAccuntDto);
    }

    public async Task<IActionResult> AccountAddressPage()
    {
        var userAccountDto = _accountService.GetUserById();
        return View(await userAccountDto);
    }
    
    public async Task<IActionResult> AccountDetailsUpdate(UserAccountEntityDto userAccountEntityDto)
    {
        ValidationResult result = await _validator.ValidateAsync(userAccountEntityDto);
        var resultName = result.Errors.Where(failure => failure.PropertyName == "Name")
            .Select(failure => failure.ErrorMessage);
        var resultSurname = result.Errors.Where(failure => failure.PropertyName == "Surname")
            .Select(failure => failure.ErrorMessage);
        var resultBirthday = result.Errors.Where(failure => failure.PropertyName == "Birthday")
            .Select(failure => failure.ErrorMessage);
        var resultPhoneNumber = result.Errors.Where(failure => failure.PropertyName == "PhoneNumber")
            .Select(failure => failure.ErrorMessage);
        
        if (!resultName.Any() && !resultSurname.Any() && !resultBirthday.Any() && !resultPhoneNumber.Any())
        {
            await _accountService.UpdateUser(userAccountEntityDto.Name, userAccountEntityDto.Surname,
                userAccountEntityDto.Birthday.ToString(CultureInfo.InvariantCulture), userAccountEntityDto.PhoneNumber);
        }

        return RedirectToAction("AccountDetailsPage", "Account");
    }

    public async Task<IActionResult> AccountPasswordUpdate(UserAccountEntityDto userAccountEntityDto, string password2)
    {
        List<string> errors = new List<string>();
        TempData["Error400"] = null;
        ValidationResult result = await _validator.ValidateAsync(userAccountEntityDto);
        var resultPassword = result.Errors.Where(failure => failure.PropertyName == "Password")
            .Select(failure => failure.ErrorMessage);

        if (!resultPassword.Any())
        {
            if (userAccountEntityDto.Password == password2)
            {
                await _accountService.UpdatePassword(userAccountEntityDto.Password, password2);
            }
            else
            {
                errors.Add("Passwords do not match.");
            }
        }


        if (!errors.Contains("") && errors.Count > 0)
        {
            TempData["Error400"] = errors.ToList();    
        }
        
        return RedirectToAction("AccountPasswordPage", "Account");
    }

    public async Task<IActionResult> AccountAddressUpdate(UserAccountEntityDto userAccountEntityDto)
    {
        ValidationResult result = await _validator.ValidateAsync(userAccountEntityDto);
        var resultProvince = result.Errors.Where(failure => failure.PropertyName == "Province")
            .Select(failure => failure.ErrorMessage);
        var resultDistrict = result.Errors.Where(failure => failure.PropertyName == "District")
            .Select(failure => failure.ErrorMessage);
        var resultFullAddress = result.Errors.Where(failure => failure.PropertyName == "FullAddress")
            .Select(failure => failure.ErrorMessage);
        var resultZipCode = result.Errors.Where(failure => failure.PropertyName == "ZipCode")
            .Select(failure => failure.ErrorMessage);

        if (!resultProvince.Any() && !resultDistrict.Any() && !resultFullAddress.Any() && !resultZipCode.Any())
        {
            await _accountService.UpdateAddress(userAccountEntityDto.Province, userAccountEntityDto.District,
                userAccountEntityDto.FullAddress, userAccountEntityDto.ZipCode.ToString());
        }
        
        return RedirectToAction("AccountAddressPage", "Account");
    }
}