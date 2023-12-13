using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCarForSale.Core.DTOs;
using MyCarForSale.Core.Entities;
using MyCarForSale.Core.Services;
using MyCarForSale.Service.Exceptions;

namespace MyCarForSale.API.Controllers;

[Authorize]
public class UserAccountController : CustomBaseController
{
    private static JwtSettings _getToken;
    private readonly IMapper _mapper;
    private readonly IGenericService<UserAccountEntity> _service;
    private readonly IAuthService _authService;

    public UserAccountController(IMapper mapper, IGenericService<UserAccountEntity> service, IAuthService authService)
    {
        _mapper = mapper;
        _service = service;
        _authService = authService;
    }
    
    [HttpGet]
    [Authorize(Roles = "Root")]
    public async Task<IActionResult> All()
    {
        var accounts = await _service.GetAllAsyncTask();
        var accountsDto = _mapper.Map<List<UserAccountEntityDto>>(accounts.ToList());
        return CreateActionResult(CustomResponseDto<List<UserAccountEntityDto>>.Success(200, accountsDto));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var account = await _service.GetByIdAsyncTask(id);

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(_getToken.Key);
        var token = jsonToken as JwtSecurityToken;
        var tokenUserId = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        var tokenUserAuthorization = token.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
        if (account.Id.ToString() != tokenUserId && tokenUserAuthorization == "User")
        {
            throw new ForbiddenException("Authorization error");
        }
        
        var accountDto = _mapper.Map<UserAccountEntityDto>(account);
        return CreateActionResult(CustomResponseDto<UserAccountEntityDto>.Success(200, accountDto));
    }

    [HttpGet("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginUser(string userEmail, string userPassword)
    {
        //var emailAndPasswordBool = await _service.AnyAsyncTask(entity => entity.Email == userEmail && entity.Password == userPassword); //Email & Password var mı diye kontrol eder.
        
        var emailAndPasswordEntity =
            await _service.SingleAsyncTask(entity => entity.Email == userEmail && entity.Password == userPassword);
        var emailAndPasswordDto = _mapper.Map<UserAccountEntityDto>(emailAndPasswordEntity);
        _getToken = await _authService.LoginUserAsync(emailAndPasswordDto);
        return CreateActionResult(CustomResponseDto<JwtSettings>.Success(200, _getToken));
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Save(UserAccountEntityDto userAccountEntityDto)
    {
        var addUserAccount = _mapper.Map<UserAccountEntity>(userAccountEntityDto);
        await _service.AddAsyncTask(addUserAccount);
        var userAccountDto = _mapper.Map<UserAccountEntityDto>(addUserAccount);
        return CreateActionResult(CustomResponseDto<UserAccountEntityDto>.Success(201, userAccountDto));
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserAccountEntityDto userAccountEntityDto)
    {
        var updateUserAccount = _mapper.Map<UserAccountEntity>(userAccountEntityDto);
        await _service.UpdateAsyncTask(updateUserAccount);
        return CreateActionResult(CustomNoContentResponseDto.Success(204));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteAccount = await _service.GetByIdAsyncTask(id);
        await _service.DeleteAsyncTask(deleteAccount);
        return CreateActionResult(CustomNoContentResponseDto.Success(204));
    }
}