﻿using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Web.Services;

namespace MyCarForSale.Web.Controllers;

public class UserController : Controller
{
    public static string? TokenKey;
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserController> _logger;
    private readonly UserAccountService _userAccountService;

    public UserController(ILogger<UserController> logger, UserAccountService userAccountService, HttpClient httpClient)
    {
        _logger = logger;
        _userAccountService = userAccountService;
        _httpClient = httpClient;
    }
    
    public IActionResult LoginAccountPage()
    {
        return View();
    }

    public async Task<IActionResult> LoginAccount(string userEmail, string userPassword)
    {
        var accountAsync = await _userAccountService.LoginAccountAsync(userEmail, userPassword);
        if (accountAsync.Key == null)
        {
            TempData["Error401"] = "email or password incorrect";
            return RedirectToAction("LoginAccountPage", "User");
        }

        TokenKey = accountAsync.Key;
        return RedirectToAction("Index", "Home");
    }
    
    
    public async Task<IActionResult> RegisterAccountPage()
    {
        var allAccount = await _userAccountService.AllUserDeneme();
        
        
        return View(allAccount);
    }
}