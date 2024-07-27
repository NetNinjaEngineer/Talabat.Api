using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Api.DTOs;
using Talabat.Api.Errors;
using Talabat.Api.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Api.Controllers;
[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto model)
    {
        if (CheckEmailExists(model.Email).Result.Value)
            return BadRequest(new ApiResponse(400, message: "User Email is Already in use."));

        var mappedUserAddress = _mapper.Map<Address>(model.Address);

        var user = new AppUser
        {
            DisplayName = model.DisplayName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            UserName = model.Email.Split('@')[0],
            Address = mappedUserAddress
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded) return BadRequest(new ApiResponse(400));

        var returnedUser = new UserDto
        {
            DisplayName = model.DisplayName,
            Email = model.Email,
            Token = await GetJwtToken(user)
        };

        return Ok(returnedUser);

    }

    [HttpPost("Login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto model)
    {
        var User = await _userManager.FindByEmailAsync(model.Email);
        if (User == null) return Unauthorized(new ApiResponse(401));
        var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);
        if (!Result.Succeeded) return Unauthorized(new ApiResponse(401));
        return Ok(new UserDto
        {
            Email = model.Email,
            DisplayName = User.DisplayName,
            Token = await GetJwtToken(User)
        });
    }

    private async Task<string> GetJwtToken(AppUser user)
        => await _tokenService.CreateTokenAsync(user, _userManager);

    [Authorize]
    [HttpGet("GetCurrentUser")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var currentUser = await _userManager.FindByEmailAsync(email!);
        var mappedUser = _mapper.Map<UserDto>(currentUser);
        mappedUser.Token = await GetJwtToken(currentUser!);
        return Ok(mappedUser);
    }

    [Authorize]
    [HttpGet("GetCurrentUserAddress")]
    public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
    {
        var currentUser = (await _userManager.GetUserWithAddressAsync(User));
        var mappedAddress = _mapper.Map<AddressDto>(currentUser?.Address);
        return Ok(mappedAddress);
    }

    [Authorize]
    [HttpPut("Address")]
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto UpdatedAddress)
    {
        var currentUser = await _userManager.GetUserWithAddressAsync(User);
        if (currentUser is null) return Unauthorized(new ApiResponse(401));
        var address = _mapper.Map<Address>(UpdatedAddress);
        address.Id = currentUser.Address.Id;
        currentUser.Address = address;
        var result = await _userManager.UpdateAsync(currentUser);
        if (!result.Succeeded) return BadRequest(new ApiResponse(400));
        return Ok(UpdatedAddress);
    }

    [HttpGet("CheckEmailExists")]
    public async Task<ActionResult<bool>> CheckEmailExists(string email)
    {
        return await _userManager.FindByEmailAsync(email) is not null;
    }

}
