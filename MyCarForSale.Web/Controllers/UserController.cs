using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Web.Services;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace MyCarForSale.Web.Controllers;

public class UserController : Controller
{
    public static string? TokenKey;
    private readonly ILogger<UserController> _logger;
    private readonly IValidator<UserAccountEntityDto> _validator;
    private readonly UserAccountService _userAccountService;

    public UserController(ILogger<UserController> logger, UserAccountService userAccountService, IValidator<UserAccountEntityDto> validator)
    {
        _logger = logger;
        _userAccountService = userAccountService;
        _validator = validator;
    }
    
    public IActionResult LoginAccountPage()
    {
        return View();
    }

    public async Task<IActionResult> LoginAccount(UserAccountEntityDto userAccountEntityDto)
    {
        var returnUrl = Url.Content("~/");
        ValidationResult result = await _validator.ValidateAsync(userAccountEntityDto);

        var passwordErrors = result.Errors.Where(failure => failure.PropertyName == "Password")
            .Select(failure => failure.ErrorMessage);
        var emailErrors = result.Errors.Where(failure => failure.PropertyName == "Email")
            .Select(failure => failure.ErrorMessage);
        
        if (!passwordErrors.Any() && !emailErrors.Any())
        {
            var accountAsync = await _userAccountService.LoginAccountAsync(userAccountEntityDto.Email, userAccountEntityDto.Password);
            if (accountAsync == null)
            {
                TempData["Error401"] = "email or password incorrect";
                return View("LoginAccountPage", userAccountEntityDto);
            }

            TokenKey = accountAsync.Key;
            return LocalRedirect(returnUrl);
        }

        return View("LoginAccountPage", userAccountEntityDto);
    }

    public async Task<IActionResult> RegisterAccountPage()
    {
        if (TokenKey == null)
        {
            return View();    
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> RegisterUser(UserAccountEntityDto userAccountEntityDto, string userRePassword)
    {
        TempData["Error400"] = null;
        ValidationResult result = await _validator.ValidateAsync(userAccountEntityDto);
        List<string> errors = new List<string>();

        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState);
            return View("RegisterAccountPage", userAccountEntityDto);
        }
        
        if (userAccountEntityDto.Password != userRePassword)
        {
            errors.Add("Passwords do not match.");
        }
        

        errors.Add(await _userAccountService.GetValidUserEmail(userAccountEntityDto.Email));

        if (!errors.Contains(""))
        {
            TempData["Error400"] = errors.ToList();
            return View("RegisterAccountPage", userAccountEntityDto);
        }

        var userAccount = await _userAccountService.PostUser(userAccountEntityDto);

        if (userAccount.IsSuccessStatusCode)
        {
            var returnUrl = Url.Content("~/");

            var accountAsync = await _userAccountService.LoginAccountAsync(userAccountEntityDto.Email, userAccountEntityDto.Password);
            if (accountAsync.Key == null)
            {
                TempData["Error401"] = "email or password incorrect";
                return RedirectToAction("LoginAccountPage", "User");
            }

            TokenKey = accountAsync.Key;

            return LocalRedirect(returnUrl);
        }
        
        return View("RegisterAccountPage", userAccountEntityDto);
    }

    public async Task<RedirectToActionResult> DeleteUser(UserAccountEntityDto userAccountEntityDto)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(TokenKey) as JwtSecurityToken;

        if (jsonToken != null)
        {
            var claims = jsonToken.Claims;
            var idClaims = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            await _userAccountService.DeleteUser(int.Parse(idClaims.Value));
        }
        
        LogoutAccount();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult  LogoutAccount()
    {
        TokenKey = null;
        
        return RedirectToAction("Index", "Home");
    }
}