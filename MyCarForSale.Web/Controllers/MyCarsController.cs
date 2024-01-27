using System.Text.Json;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Web.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace MyCarForSale.Web.Controllers;

/*
 * Support Upload Photo Formats
 * 1. JPEG => All photos convert to jpeg.
 * 2. HEIC => This format is default for Iphone and Samsung
 * 3. Raw => This format is alternatively using for Samsung and photo cameras.
 */

public class MyCarCustomList
{
    public List<MainClassificationEntityDto> MainClassificationEntityDtos { get; set; }
    public CarFeaturesEntityDto CarFeaturesEntityDto { get; set; }
    public List<CarImagesEntityDto> ImagesEntityDtos { get; set; }
}

public class MyCarsController : Controller
{
    private readonly MyCarsService _myCarsService;

    public MyCarsController(MyCarsService myCarsService)
    {
        _myCarsService = myCarsService;
    }

    public async Task<ViewResult> PostCarPage()
    {
        var userId = await _myCarsService.GetUserId();
        if (userId != null)
        {
            ViewBag.UserId = userId;
        }

        var getAllClassification = await _myCarsService.GetAllClassification();

        var customList = new MyCarCustomList
        {
            MainClassificationEntityDtos = getAllClassification
        };

        return View(customList);
    }

    public async Task<IActionResult> PostConfiguration(string classification, string make, string model, string package,
        string year)
    {
        ushort uYear = UInt16.Parse(year);
        var myViewModel = await _myCarsService.PostClassification(classification, make, model, package, uYear);
        return Json(myViewModel);
    }

    public async Task<IActionResult> PostCarDetails(CarFeaturesEntityDto carFeaturesEntityDto)
    {
        var postCarFeaturesEntity = await _myCarsService.PostCarFeatures(carFeaturesEntityDto);

        if (postCarFeaturesEntity != null)
        {
            TempData["carFeaturesEntityId"] = postCarFeaturesEntity.Id;
            TempData.Peek("carFeaturesEntityId");
            return RedirectToAction("AddImagePage", "MyCars");
        }

        return RedirectToAction("PostCarPage", "MyCars");
    }

    public async Task<IActionResult> AddImagePage()
    {
        return View();
    }

    public async Task<IActionResult> PostImageUrl(IFormFile image)
    {
        var randomPath = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileName(image.FileName));
        using var imageLoad = Image.Load(image.OpenReadStream());
        imageLoad.Mutate(x => x.Resize(1614, 1242, KnownResamplers.Lanczos3));

        var encoder = new JpegEncoder
        {
            Quality = 90
        };

        await imageLoad.SaveAsync(randomPath, encoder);
        bool success = Int32.TryParse(TempData.Peek("carFeaturesEntityId")?.ToString(), out var uploadCarId);
        
        if (uploadCarId != 0 || success)
        {
            string url = await _myCarsService.PostImageKitWebsite(randomPath, Path.GetFileName(image.FileName));
            
            var imageDto = new CarImagesEntityDto
            {
                BaseEntityId = uploadCarId,
                CarImageUrl = url
            };
            
            await _myCarsService.PostImageWithDatabase(imageDto);
        }
        
        System.IO.File.Delete(randomPath);
        return Json("");
    }

    public async Task<IActionResult> ListPage()
    {
        var userAllCars = await _myCarsService.GetSingleUserCars();

        return View(userAllCars);
    }

    public async Task<IActionResult> DeleteCarOnListPage(int userId, int carId)
    {
        bool boolDeleted = await _myCarsService.DeleteMySelectCar(userId, carId);

        return RedirectToAction("ListPage", "MyCars");
    }
}