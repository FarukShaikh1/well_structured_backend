using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace FMS_Collection.API.Controllers;

// OTP endpoints are public — they are part of the authentication flow
[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
[Produces("application/json")]
public class OtpController(OtpService otpService) : ControllerBase
{
    [HttpPost("send")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Send([FromBody] SendOtpRequest request)
    {
        var result = await otpService.SendAsync(request);
        return Ok(result);
    }

    [HttpPost("verify")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Verify([FromBody] VerifyOtpRequest request)
    {
        var result = await otpService.VerifyAsync(request);
        return Ok(result);
    }

    [HttpPost("reset-password")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordWithOtpRequest request)
    {
        var result = await otpService.ResetPasswordWithOtpAsync(request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}


