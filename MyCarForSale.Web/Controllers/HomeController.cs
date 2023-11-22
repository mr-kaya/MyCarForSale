using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Web.Models;
using MyCarForSale.Web.Services;

namespace MyCarForSale.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CarFeaturesWithImageAndClassificationAndUserAccountService _carFeaturesService;
    
    public HomeController(ILogger<HomeController> logger, CarFeaturesWithImageAndClassificationAndUserAccountService carFeaturesService)
    {
        _logger = logger;
        _carFeaturesService = carFeaturesService;
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public async Task<IActionResult> Index()
    {
        var cars = await _carFeaturesService.AllSaleCarsAsync();
        Random random = new Random();
        for (int i = 0; i < cars.Count; i++)
        {
            int randomIndex = random.Next(cars.Count());
            (cars[i], cars[randomIndex]) = (cars[randomIndex], cars[i]);
        }
        return View(cars);
    }

    public async Task<IActionResult> Remove(int id)
    {
        await _carFeaturesService.RemoveAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}