using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Web.Models;
using MyCarForSale.Web.Services;

namespace MyCarForSale.Web.Controllers;

public class MainPageFilterVariable
{
    public List<string> carClassification { get; set; }
    public int  pageIndex { get; set; }
    public int pageSize { get; set; }
}

public class MyViewModel
{
    public IEnumerable<CarFeaturesWithImageAndClassificationAndUserAccountDto> CarFeatures { get; set; }
    public List<string>? MainClassification { get; set; }
    
    public int InfiniteScrollCount { get; set; }
}

public class HomeController : Controller
{
    private static List<string> ClassificationMenuItemsList = new();
    private const int PageSize = 7;
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
    
    [ResponseCache(NoStore = true, Duration = 0)]
    public async Task<IActionResult> Index()
    {
        var page = Request.Query["page"].ToString();
        page = System.Net.WebUtility.UrlDecode(page);
        page = page.TrimStart('/');
        
        MainPageFilterVariable model = new()
        {
            carClassification = ClassificationMenuItemsList,
            pageIndex = 1,
            pageSize = PageSize
        };
        
        if (ClassificationMenuItemsList != null)
        {
            if (page != "")
            {
                ClassificationMenuItemsList.Add(page);
                
                var i = ClassificationMenuItemsList.IndexOf(page);

                for (int k = i + 1; k < ClassificationMenuItemsList.Count;)
                {
                    ClassificationMenuItemsList.RemoveAt(k);
                }
            }
            else
            {
                ClassificationMenuItemsList.Clear();
            }
        }
        
        var getAllData = await _carFeaturesService.AllSaleCarsAsync();
        var firstPage = await _carFeaturesService.GetSalePageWithId(model);
        var classification = await _carFeaturesService.AllClassificationAsync();
        
        _infiniteScrollCount = (getAllData.Count / PageSize) + 1;
        
        Random random = new Random();

        if (ClassificationMenuItemsList.Count == 0)
        {
            for (int i = 0; i < firstPage.Count; i++)
            {
                int randomIndex = random.Next(firstPage.Count());
                (firstPage[i], firstPage[randomIndex]) = (firstPage[randomIndex], firstPage[i]);
            }
        }
    
        List<string> classificationList = new List<string>();
        
        
        if (ClassificationMenuItemsList.Count == 0)
        {
            foreach (var item in classification)
            {
                int index = classificationList.FindIndex(a => a.Contains(item.MainClassification));
                if (index == -1) 
                {
                    classificationList.Add(item.MainClassification);
                }
            }
        }
        else if (ClassificationMenuItemsList.Count == 1)
        {
            foreach (var item in classification)
            {
                if (item.MainClassification == ClassificationMenuItemsList[^1])
                {
                    int index = classificationList.FindIndex(a => a.Contains(item.CarBrand));
                    if (index == -1) 
                    {
                        classificationList.Add(item.CarBrand);
                    }
                }   
            }
        }
        else if (ClassificationMenuItemsList.Count == 2)
        {
            foreach (var item in classification)
            {
                if (item.CarBrand == ClassificationMenuItemsList[^1] && item.MainClassification == ClassificationMenuItemsList[^2])
                {
                    int index = classificationList.FindIndex(a => a.Contains(item.CarModel));
                    if (index == -1) 
                    {
                        classificationList.Add(item.CarModel);
                    }   
                }
            }
        }
        else if (ClassificationMenuItemsList.Count == 3)
        {
            foreach (var item in classification)
            {
                if (item.CarModel == ClassificationMenuItemsList[^1] && item.CarBrand == ClassificationMenuItemsList[^2] && item.MainClassification == ClassificationMenuItemsList[^3])
                {
                    int index = classificationList.FindIndex(a => a.Contains(item.CarPackage));
                    if (index == -1) 
                    {
                        classificationList.Add(item.CarPackage);
                    }   
                }
            }
        }
        else if (ClassificationMenuItemsList.Count == 4)
        {
            foreach (var item in classification)
            {
                if (item.CarPackage == ClassificationMenuItemsList[^1] && item.CarModel == ClassificationMenuItemsList[^2] && item.CarBrand == ClassificationMenuItemsList[^3] && item.MainClassification == ClassificationMenuItemsList[^4])
                {
                    int index = classificationList.FindIndex(a => a.Contains(item.CarYear.ToString()));
                    if (index == -1) 
                    {
                        classificationList.Add(item.CarYear.ToString());
                    }   
                }
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
        MainPageFilterVariable model = new()
        {
            carClassification = ClassificationMenuItemsList,
            pageIndex = pageIndex,
            pageSize = PageSize
        };
        
        var getPageData = await _carFeaturesService.GetSalePageWithId(model);
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