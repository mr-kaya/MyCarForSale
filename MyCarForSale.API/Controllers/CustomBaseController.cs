using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Core.DTOs;

namespace MyCarForSale.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomBaseController : ControllerBase
{
    [NonAction]
    public IActionResult CreateActionResult(CustomNoContentResponseDto responseDto)
    {
        return new ObjectResult(null)
        {
            StatusCode = responseDto.StatusCode
        };
    }
    
    [NonAction]
    public IActionResult CreateActionResult<T>(CustomResponseDto<T> responseDto)
    {
        if (responseDto.StatusCode == 204)
        {
            return new ObjectResult(null)
            {
                StatusCode = responseDto.StatusCode
            };
        }

        return new ObjectResult(responseDto)
        {
            StatusCode = responseDto.StatusCode
        };
    }
}