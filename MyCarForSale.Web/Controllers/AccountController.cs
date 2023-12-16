using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Web.Services;

namespace MyCarForSale.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserAccountService _accountService;

    public AccountController(UserAccountService accountService)
    {
        _accountService = accountService;
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
    
    public async Task<IActionResult> AccountDetailsUpdate(string email, string name, string surname, string birthday, string phone)
    {
        await _accountService.UpdateUser(email, name, surname, birthday, phone);
        return RedirectToAction("AccountDetailsPage", "Account");
    }

    public async Task<IActionResult> AccountPasswordUpdate(string password1, string password2)
    {
        await _accountService.UpdatePassword(password1, password2);
        return RedirectToAction("AccountPasswordPage", "Account");
    }

    public async Task<IActionResult> AccountAddressUpdate(string province, string district, string fullAddress, string zipCode)
    {
        await _accountService.UpdateAddress(province, district, fullAddress, zipCode);
        return RedirectToAction("AccountAddressPage", "Account");
    }
}