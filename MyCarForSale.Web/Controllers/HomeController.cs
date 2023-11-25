using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Web.Models;
using MyCarForSale.Web.Services;

namespace MyCarForSale.Web.Controllers;

public class MyViewModel
{
    public IEnumerable<CarFeaturesWithImageAndClassificationAndUserAccountDto> CarFeatures { get; set; }
    public List<string>? MainClassification { get; set; }
}

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
        var classification = await _carFeaturesService.AllClassificationAsync();
        Random random = new Random();
        
        for (int i = 0; i < cars.Count; i++)
        {
            int randomIndex = random.Next(cars.Count());
            (cars[i], cars[randomIndex]) = (cars[randomIndex], cars[i]);
        }
    
        List<string> classificationList = new List<string>();
        foreach (var item in classification)
        {
            int index = classificationList.FindIndex(a => a.Contains(item.MainClassification));
            if (index == -1) 
            {
                classificationList.Add(item.MainClassification);
            }
        }

        var myViewModel = new MyViewModel
        {
            CarFeatures = cars,
            MainClassification = classificationList
        };
        //return View(new {CarFeatures = cars, MainClassification = classification});
        return View(myViewModel);
    }

    public async Task<IActionResult> CarId(int id)
    {
        var getIdSaleCar = await _carFeaturesService.GetSaleByIdAsync(id);
        return View(getIdSaleCar);
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