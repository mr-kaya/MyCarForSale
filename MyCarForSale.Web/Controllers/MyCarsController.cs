using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
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
    private readonly IValidator<CarFeaturesEntityDto> _carValidator;
    private readonly IValidator<MainClassificationEntityDto> _mainClassificationValidator;

    public MyCarsController(MyCarsService myCarsService, IValidator<MainClassificationEntityDto> mainClassificationValidator, IValidator<CarFeaturesEntityDto> carValidator)
    {
        _myCarsService = myCarsService;
        _mainClassificationValidator = mainClassificationValidator;
        _carValidator = carValidator;
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
        MainClassificationEntityDto mainClassificationEntityDto = new()
        {
            MainClassification = classification,
            CarBrand = make,
            CarModel = model,
            CarPackage = package,
            CarYear = ushort.Parse(year)
        };

        ValidationResult result = await _mainClassificationValidator.ValidateAsync(mainClassificationEntityDto);
        if (result.IsValid)
        {
            ushort uYear = UInt16.Parse(year);
            var myViewModel = await _myCarsService.PostClassification(classification, make, model, package, uYear);
            return Json(myViewModel);   
        }
        
        return RedirectToAction("PostCarPage", "MyCars");
    }

    public async Task<IActionResult> PostCarDetails(CarFeaturesEntityDto carFeaturesEntityDto)
    {
        ValidationResult result = await _carValidator.ValidateAsync(carFeaturesEntityDto);
        
        if (result.IsValid)
        {
            var postCarFeaturesEntity = await _myCarsService.PostCarFeatures(carFeaturesEntityDto);
            
            if (postCarFeaturesEntity != null)
            {
                TempData["userId"] = postCarFeaturesEntity.PublishUserId;
                TempData["carId"] = postCarFeaturesEntity.Id;
                TempData["mainClassificationId"] = postCarFeaturesEntity.MainClassificationEntityId;
            
                return RedirectToAction("AddImagePage", "MyCars");
            }    
        }
        
        return RedirectToAction("PostCarPage", "MyCars");
    }

    public async Task<IActionResult> UpdateCarDetails(CarFeaturesWithImageAndClassificationAndUserAccountDto carFeaturesWithImageAndClassificationAndUserAccountDto)
    {
        var updateFeaturesEntity = await _myCarsService.GetCarImagesWithId(carFeaturesWithImageAndClassificationAndUserAccountDto.Id, carFeaturesWithImageAndClassificationAndUserAccountDto.PublishUserId.ToString());
        
        var getCarMainClass =
            await _myCarsService.GetClassificationWithCarId(carFeaturesWithImageAndClassificationAndUserAccountDto.MainClassificationEntityId, carFeaturesWithImageAndClassificationAndUserAccountDto.PublishUserId);
        
        var carFeaturesWithImageAndClassificationDto = new CarFeaturesWithImageAndClassificationDto
        {
            MainClassificationEntity = getCarMainClass,
            CarImagesEntities = updateFeaturesEntity,
            
            MainClassificationEntityId = carFeaturesWithImageAndClassificationAndUserAccountDto.MainClassificationEntityId,
            Price = carFeaturesWithImageAndClassificationAndUserAccountDto.Price,
            AdvertisementName = carFeaturesWithImageAndClassificationAndUserAccountDto.AdvertisementName,
            AdvertisementDescription = carFeaturesWithImageAndClassificationAndUserAccountDto.AdvertisementDescription,
            CarDrivetrain = carFeaturesWithImageAndClassificationAndUserAccountDto.CarDrivetrain,
            CarGuarantee = carFeaturesWithImageAndClassificationAndUserAccountDto.CarGuarantee,
            EngineDisplacement = carFeaturesWithImageAndClassificationAndUserAccountDto.EngineDisplacement,
            PublishUserId = carFeaturesWithImageAndClassificationAndUserAccountDto.PublishUserId,
            EngineTorque = carFeaturesWithImageAndClassificationAndUserAccountDto.EngineTorque,
            TransmissionType = carFeaturesWithImageAndClassificationAndUserAccountDto.TransmissionType,
            CarTotalKm = carFeaturesWithImageAndClassificationAndUserAccountDto.CarTotalKm,
            EngineFuelType = carFeaturesWithImageAndClassificationAndUserAccountDto.EngineFuelType,
            EngineHorsePower = carFeaturesWithImageAndClassificationAndUserAccountDto.EngineHorsePower,
            Id = carFeaturesWithImageAndClassificationAndUserAccountDto.Id
        };

        var boolFinal = await _myCarsService.UpdateCarFeatures(carFeaturesWithImageAndClassificationDto);

        if (boolFinal)
        {
            TempData["userId"] = carFeaturesWithImageAndClassificationAndUserAccountDto.PublishUserId;
            TempData["carId"] = carFeaturesWithImageAndClassificationAndUserAccountDto.Id;
            TempData["mainClassificationId"] = carFeaturesWithImageAndClassificationAndUserAccountDto.MainClassificationEntityId;
            return RedirectToAction("AddImagePage", "MyCars");
        }

        return RedirectToAction("ListPage", "MyCars");
    }

    public async Task<IActionResult> DeleteImage(int imageId, int userId)
    {
        bool boolDeletedPage = await _myCarsService.DeleteImageWithId(imageId, userId);
        return Json(new {success = boolDeletedPage});
    }
    
    public async Task<IActionResult> AddImagePage()
    {
        var userId = Int32.Parse(TempData["userId"]!.ToString() ?? string.Empty);
        var mainClassificationId = Int32.Parse(TempData["mainClassificationId"]!.ToString() ?? string.Empty);
        var carFeaturesId = Int32.Parse(TempData["carId"]!.ToString() ?? string.Empty);

        TempData.Keep("userId");
        TempData.Keep("mainClassificationId");
        TempData.Keep("carId");
        
        var carFeaturesEntityDto = await _myCarsService.GetCarWithId(carFeaturesId);
        
        var updateFeaturesEntity = await _myCarsService.GetCarImagesWithId(carFeaturesId, userId.ToString());

        
        var getCarMainClass =
            await _myCarsService.GetClassificationWithCarId(mainClassificationId, userId);
        
        var carFeaturesWithImageAndClassificationDto = new CarFeaturesWithImageAndClassificationDto
        {
            MainClassificationEntity = getCarMainClass,
            CarImagesEntities = updateFeaturesEntity,
            
            MainClassificationEntityId = carFeaturesEntityDto.MainClassificationEntityId,
            Price = carFeaturesEntityDto.Price,
            AdvertisementName = carFeaturesEntityDto.AdvertisementName,
            AdvertisementDescription = carFeaturesEntityDto.AdvertisementDescription,
            CarDrivetrain = carFeaturesEntityDto.CarDrivetrain,
            CarGuarantee = carFeaturesEntityDto.CarGuarantee,
            EngineDisplacement = carFeaturesEntityDto.EngineDisplacement,
            PublishUserId = carFeaturesEntityDto.PublishUserId,
            EngineTorque = carFeaturesEntityDto.EngineTorque,
            TransmissionType = carFeaturesEntityDto.TransmissionType,
            CarTotalKm = carFeaturesEntityDto.CarTotalKm,
            EngineFuelType = carFeaturesEntityDto.EngineFuelType,
            EngineHorsePower = carFeaturesEntityDto.EngineHorsePower,
            Id = carFeaturesEntityDto.Id
        };
        
        return View(carFeaturesWithImageAndClassificationDto);
    }

    public async Task<IActionResult> PostImageUrl(IFormFile image, int carFeaturesEntityId)
    {
        bool boolUploaded = false;
        var randomPath = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileName(image.FileName));
        using var imageLoad = Image.Load(image.OpenReadStream());
        imageLoad.Mutate(x => x.Resize(1614, 1242, KnownResamplers.Lanczos3));

        var encoder = new JpegEncoder
        {
            Quality = 90
        };

        await imageLoad.SaveAsync(randomPath, encoder);
        
        if (carFeaturesEntityId != 0)
        {
            var carFeaturesEntityDto = await _myCarsService.GetCarWithId(carFeaturesEntityId);
            var allImages =
                await _myCarsService.GetCarImagesWithId(carFeaturesEntityId,
                    carFeaturesEntityDto.PublishUserId.ToString());
            
            if (allImages.Count < 12)
            {
                string url = await _myCarsService.PostImageKitWebsite(randomPath, Path.GetFileName(image.FileName));
            
                var imageDto = new CarImagesEntityDto
                {
                    BaseEntityId = carFeaturesEntityId,
                    CarImageUrl = url
                };
            
                boolUploaded = await _myCarsService.PostImageWithDatabase(imageDto);   
            }
        }
        
        System.IO.File.Delete(randomPath);
        return Json(new {success = boolUploaded});
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
        //return Json(new { success = boolDeleted});
    }

    public async Task<IActionResult> EditCarPage(int id)
    {
        var saleCarWithId = await _myCarsService.GetSaleCarWithId(id);

        if (saleCarWithId != null)
        {
            return View(saleCarWithId);    
        }

        return RedirectToAction("ListPage", "MyCars");
    }
}