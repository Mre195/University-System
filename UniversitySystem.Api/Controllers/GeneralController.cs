using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Api.Controllers;
using UniversitySystem.Services.DTOs.Shared.Auth;
using UniversitySystem.Services.Services.Shared;

[ApiController]
public partial class GeneralController : BaseApiController
{
    private readonly LoginService _loginService;
    private readonly GetProfileService _getProfile;
    public GeneralController(LoginService loginService, GetProfileService getProfile)
    {
        _loginService = loginService;
        _getProfile = getProfile;
    }
    [AllowAnonymous]
    [HttpPost("api/login")]
    public async Task<IActionResult> LoginApi([FromBody] LoginDto loginDto)
    {
        try
        {
            var token = await _loginService.LoginAsync(loginDto);
            return Ok(new { Token = token });
        }
        catch (InvalidOperationException)
        {
            return Unauthorized(new { Message = "Invalid Email or Password" });
        }
    }


    [Authorize]
    [HttpGet("api/profile")]
    public async Task<IActionResult> GetProfileApi() =>
        Ok(await _getProfile.GetProfileAsync(GetId(), GetMyRole()));


    [AllowAnonymous]
    [HttpPost("api/hashed-password")]
    public IActionResult GetHashedPassword([FromBody] string password)
    {
        var hashedPass = BCrypt.Net.BCrypt.HashPassword(password);
        return Ok(hashedPass);
    }



}