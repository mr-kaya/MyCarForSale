using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Web.Services;

namespace MyCarForSale.Web.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly UserAccountService _userAccountService;

    public UserController(ILogger<UserController> logger, UserAccountService userAccountService)
    {
        _logger = logger;
        _userAccountService = userAccountService;
    }
    
    public IActionResult LoginAccountPage()
    {
        return View();
    }

    public async Task<IActionResult> LoginAccount(string userEmail, string userPassword)
    {
        var accountAsync = await _userAccountService.LoginAccountAsync(userEmail, userPassword);
        if (accountAsync == null)
        {
            TempData["Error401"] = "email or password incorrect";
            return RedirectToAction("LoginAccountPage", "User");
        }
        return Json(accountAsync);
    }
    
    
    public IActionResult RegisterAccountPage()
    {
        return View();
    }
}