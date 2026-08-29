using FMS_Collection.API.Authorization;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FMS_Collection.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UserRegistrationController(
    IUserRegistrationRepository registrationRepository, OtpService otpService)
    : ControllerBase
{
    private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    [AllowAnonymous]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Register([FromForm] UserRegistrationRequest request)
    {
        try
        {
            // =========================
            // VALIDATION
            // =========================

            if (request.DateOfBirth == default)
            {
                return BadRequest(new { message = "Date of birth is required." });
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new { message = "Name is required." });
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { message = "Email is required." });
            }


            // =========================
            // NORMALIZE EMAIL
            // =========================

            var email = request.Email.Trim().ToLowerInvariant();


            // =========================
            // CHECK EXISTING REGISTRATION
            // =========================

            var existing = await registrationRepository.GetByEmailAsync(email);

            if (existing != null)
            {
                if (existing.Status == "Approved")
                {
                    return BadRequest(new
                    {
                        message = "An account already exists with this email address."
                    });
                }
                if (existing.Status == "PendingEmailVerification" || existing.Status == "PendingApproval")
                {
                    return BadRequest(new
                    {
                        message = "A registration already exists for this email address."
                    });
                }
            }

            // =========================
            // GENERATE OTP
            // =========================

            var otp = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            // =========================
            // HASH OTP
            // =========================

            //var otpHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(otp)));
            var otpHash = EncryptDecryptAlgorithm.Encrypt(otp);
            var otpExpiry = DateTime.Now.AddMinutes(10);

            // =========================
            // PROFILE PHOTO
            // =========================

            string? profilePhotoPath = null;

            if (request.ProfilePhoto != null)
            {
                // For now only keep the file name.
                // Replace this with Azure Blob upload.

                profilePhotoPath = request.ProfilePhoto.FileName;
            }


            // =========================
            // CREATE REGISTRATION
            // =========================

            var registration = new UserRegistration
            {
                DateOfBirth = request.DateOfBirth,
                Name = request.Name.Trim(),
                Email = email,
                MobileNumber = request.MobileNumber?.Trim(),
                Address = request.Address?.Trim(),
                ProfilePhoto = profilePhotoPath,
                EmailVerificationOtp = otpHash,
                OtpExpiry = otpExpiry,
                EmailVerified = false,
                Status = "PendingEmailVerification"
            };
            var registrationId = await registrationRepository.AddAsync(registration);

            // =========================
            // SEND OTP EMAIL
            // =========================

            SendEmailOtpRequest otpRequest = new SendEmailOtpRequest { EmailId = email, Purpose = "verification" };

            await otpService.StoreOtpAsync(otpRequest, null);

            return Ok(new
            {
                registrationId,
                message = "Registration submitted successfully. Please check your email for the verification OTP."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
        }
    }

    // =========================================================
    // VERIFY EMAIL
    // =========================================================

    [AllowAnonymous]
    [HttpPost("VerifyEmail")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        try
        {
            if (request.RegistrationId == Guid.Empty)
            {
                return BadRequest(new { error = "Registration ID is required." });
            }

            if (string.IsNullOrWhiteSpace(request.Otp))
            {
                return BadRequest(new { error = "OTP is required." });
            }

            if (request.Otp.Trim().Length != 6)
            {
                return BadRequest(new { error = "OTP must be 6 digits." });
            }

            var verified = await registrationRepository.VerifyEmailAsync(request.RegistrationId, request.Otp.Trim());

            if (!verified)
            {
                return BadRequest(new { error = "Invalid or expired OTP." });
            }

            // Email successfully verified.
            // Registration status should now be:
            // PendingApproval

            // TODO:
            // Create notification for Super Admin

            return Ok(new
            {
                success = true,
                message = "Email verified successfully. " + "Your registration is now pending administrator approval."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new{error = ex.Message,stackTrace = ex.StackTrace});
        }
    }

    // =========================================================
    // PENDING APPROVAL
    // =========================================================
    [Authorize]
    [HttpGet("Pending")]
    [RequirePermission("User.Create")]
    public async Task<IActionResult> GetPending()
    {
        try
        {
            var result = await registrationRepository.GetPendingApprovalAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }


    // =========================================================
    // DETAILS
    // =========================================================

    [Authorize]
    [HttpGet("{registrationId:guid}")]
    [RequirePermission("User.Create")]
    public async Task<IActionResult> GetDetails(Guid registrationId)
    {
        try
        {
            var result = await registrationRepository.GetDetailsAsync(registrationId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    // =====================================================
    // APPROVE USER
    // =====================================================

    [HttpPost("Approve/{registrationId:guid}")]
    [Authorize]
    [RequirePermission("User.Create")]
    public async Task<IActionResult> Approve(Guid registrationId)
    {
        try
        {
            var success = await registrationRepository.ApproveAsync(registrationId, CurrentUserId);

            if (!success)
            {
                return BadRequest(new
                {
                    error = "Unable to approve this registration. " + "It may not exist or may already be processed."
                });
            }

            return Ok(new
            {
                success = true,
                message = "User approved successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }


    // =====================================================
    // REJECT USER
    // =====================================================

    [HttpPost("{registrationId:guid}/Reject")]
    [Authorize]
    [RequirePermission("User.Create")]
    public async Task<IActionResult> Reject(Guid registrationId, [FromBody] RejectUserRegistrationRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Reason))
            {
                return BadRequest(new
                {
                    error = "Rejection reason is required."
                });
            }

            var success = await registrationRepository.RejectAsync(registrationId, request.Reason.Trim(), CurrentUserId);

            if (!success)
            {
                return BadRequest(new
                {
                    error = "Unable to reject this registration. " + "It may not exist or may already be processed."
                });
            }

            return Ok(new
            {
                success = true,
                message = "User registration rejected successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }
}
