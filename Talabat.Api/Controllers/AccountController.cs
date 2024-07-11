using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.DTOs;
using Talabat.Api.Errors;
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

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto model)
    {
        var user = new AppUser
        {
            DisplayName = model.DisplayName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            UserName = model.Email.Split('@')[0]
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

}
