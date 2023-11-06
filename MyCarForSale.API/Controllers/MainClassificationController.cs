using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Entities;
using MyCarForSale.Core.Services;

namespace MyCarForSale.API.Controllers;

public class MainClassificationController : CustomBaseController
{
    private readonly IMapper _mapper;
    private readonly IGenericService<MainClassificationEntity> _service;


    public MainClassificationController(IMapper mapper, IGenericService<MainClassificationEntity> service)
    {
        _mapper = mapper;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        var classification = await _service.GetAllAsyncTask();
        var classificationDto = _mapper.Map<List<MainClassificationEntityDto>>(classification.ToList());
        return CreateActionResult(CustomResponseDto<List<MainClassificationEntityDto>>.Success(200, classificationDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var getClassification = await _service.GetByIdAsyncTask(id);
        var getClassificationDto = _mapper.Map<MainClassificationEntityDto>(getClassification);
        return CreateActionResult(CustomResponseDto<MainClassificationEntityDto>.Success(200, getClassificationDto));
    }

    [HttpPost]
    public async Task<IActionResult> Save(MainClassificationEntityDto mainClassificationEntityDto)
    {
        var classification = _mapper.Map<MainClassificationEntity>(mainClassificationEntityDto);
        await _service.AddAsyncTask(classification);
        var classificationDto = _mapper.Map<MainClassificationEntityDto>(classification);
        return CreateActionResult(CustomResponseDto<MainClassificationEntityDto>.Success(201, classificationDto));
    }

    [HttpPut]
    public async Task<IActionResult> Update(MainClassificationEntityDto mainClassificationEntityDto)
    {
        var classification = _mapper.Map<MainClassificationEntity>(mainClassificationEntityDto);
        await _service.UpdateAsyncTask(classification);
        return CreateActionResult(CustomNoContentResponseDto.Success(204));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var classification = await _service.GetByIdAsyncTask(id);
        await _service.DeleteAsyncTask(classification);
        return CreateActionResult(CustomNoContentResponseDto.Success(204));
    }
}