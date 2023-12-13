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
}