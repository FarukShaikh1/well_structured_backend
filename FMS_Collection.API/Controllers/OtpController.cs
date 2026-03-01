using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Mvc;

namespace FMS_Collection.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly OtpService _otpService;
        public OtpController(OtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpGet("SendWelcomeEmailAsync")]
        public async Task<IActionResult> SendWelcomeEmailAsync(string emailAddress, string planePassword)
        {
            var result = await _otpService.SendWelcomeEmailAsync(emailAddress, planePassword);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("ResetPasswordWithOtpAsync")]
        public async Task<IActionResult> ResetPasswordWithOtpAsync(ResetPasswordWithOtpRequest request)
        {
            var result = await _otpService.ResetPasswordWithOtpAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendOtpRequest request)
        {
            var result = await _otpService.SendAsync(request);
            //if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] VerifyOtpRequest request)
        {
            var result = await _otpService.VerifyAsync(request);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordWithOtpRequest request)
        {
            var result = await _otpService.ResetPasswordWithOtpAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}


