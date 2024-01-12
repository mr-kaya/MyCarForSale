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
    
    public int InfiniteScrollCount { get; set; }
}

public class HomeController : Controller
{
    private const int PageSize = 3;
    private static int _infiniteScrollCount;
    
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
        var getAllData = await _carFeaturesService.AllSaleCarsAsync();
        var firstPage = await _carFeaturesService.GetSalePageWithId(1, PageSize);
        var classification = await _carFeaturesService.AllClassificationAsync();
        
        _infiniteScrollCount = (getAllData.Count / PageSize) + 1;
        
        Random random = new Random();
        
        for (int i = 0; i < firstPage.Count; i++)
        {
            int randomIndex = random.Next(firstPage.Count());
            (firstPage[i], firstPage[randomIndex]) = (firstPage[randomIndex], firstPage[i]);
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
            CarFeatures = firstPage,
            MainClassification = classificationList,
            InfiniteScrollCount = _infiniteScrollCount
        };
        return View(myViewModel);
    }

    /*
     * MainPage, infinite scroll for table.
     */
    public async Task<IActionResult> LoadMorePage(int pageIndex)
    {
        var getPageData = await _carFeaturesService.GetSalePageWithId(pageIndex, PageSize);
        var classification = await _carFeaturesService.AllClassificationAsync();
        
        
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
            CarFeatures = getPageData,
            MainClassification = classificationList,
            InfiniteScrollCount = _infiniteScrollCount
        };
        
        return Json(myViewModel);
    }

    public async Task<IActionResult> ChangeMainMenu(string selectedOption)
    {
        
    }

    /*
     * Selected car ad details page.
     */
    public async Task<IActionResult> CarId(int id)
    {
        var getIdSaleCar = await _carFeaturesService.GetSaleWithIdAsync(id);
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