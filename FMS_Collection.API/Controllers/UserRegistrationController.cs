using FMS_Collection.API.Authorization;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static FMS_Collection.Core.Constants.Constants;

namespace FMS_Collection.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UserRegistrationController(
    IUserRegistrationRepository registrationRepository, OtpService otpService,
        EmailService emailService, UserService userService
    )
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

            var otp = RandomGeneratorService.GenerateNumericOtp(6);
            SendEmailOtpRequest otpRequest = new SendEmailOtpRequest { EmailId = email, OtpCode = otp, Purpose = "Registration" };
            await otpService.StoreOtpAsync(otpRequest, null);

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

            await emailService.SendAsync(
                request.Email,
                EmailTemplateCodes.VerifyEmail,
                new Dictionary<string, string>
                {
                    ["UserName"] = request.Name,
                    ["Email"] = request.Email,
                    ["VerificationCode"] = otp,
                    ["VerificationLink"] = AppSettings.SiteLiveUrl + "email-verification?registrationId=" + registrationId + "&verificationOtp=" + otp
                });

            // =========================
            // SEND OTP EMAIL
            // =========================
            // 1. OTP Verification should sent to new registered user
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.OtpVerification,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email,
            //        ["Email"] = email,
            //        ["Otp"] = otp,
            //        ["ExpiryMinutes"] = "10",
            //        ["PurposeText"] = "Login",
            //        ["Year"] = DateTime.UtcNow.Year.ToString(),
            //        ["SupportEmail"] = AppSettings.OwnerEmail
            //    });


            //// 2. Registration Submitted should sent to user after registration otp verification
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.RegistrationSubmitted,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email,
            //        ["Email"] = email
            //    });


            //// 3. Email Verified should sent to user after admin approved request
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.EmailVerified,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email,
            //        ["Email"] = email,
            //        ["AppUrl"] = AppSettings.SiteLiveUrl
            //    });


            //// 4. New Registration should sent to admin as notification for new user
            //await emailService.SendAsync(
            //    AppSettings.OwnerEmail,
            //    EmailTemplateCodes.NewRegistrationAdmin,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email,
            //        ["Email"] = email,
            //        ["DateOfAction"] = DateTime.Now.ToString("dd MMM yyyy hh:mm tt"),
            //        ["AdminUrl"] = AppSettings.SiteLiveUrl
            //    });


            //// 5. Registration Approved should sent to user abo
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.RegistrationApproved,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email,
            //        ["AppUrl"] = AppSettings.SiteLiveUrl
            //    });


            //// 6. Registration Rejected
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.RegistrationRejected,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email,
            //        ["Reason"] = "rejectionReason"
            //    });


            //// 7. Password Reset OTP
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.PasswordResetOtp,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email,
            //        ["Otp"] = otp,
            //        ["ExpiryMinutes"] = "10",
            //        ["Year"] = DateTime.UtcNow.Year.ToString(),
            //        ["SupportEmail"] = AppSettings.OwnerEmail
            //    });




            //// 9. Account Locked
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.AccountLocked,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email
            //    });


            //// 10. Login Alert
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.LoginAlert,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email,
            //        ["LoginDateTime"] = DateTime.Now.ToString("dd MMM yyyy hh:mm tt"),
            //        ["IpAddress"] = "ipAddress"
            //    });


            //// 11. Account Activated
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.AccountActivated,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email,
            //        ["AppUrl"] = AppSettings.SiteLiveUrl
            //    });


            //// 12. Account Deactivated
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.AccountDeactivated,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email,
            //        ["Reason"] = "reason"
            //    });


            //// 13. Role Changed
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.RoleChanged,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email,
            //        ["RoleName"] = "roleName"
            //    });


            //// 14. New User Created
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.NewUserCreated,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email,
            //        ["Email"] = email,
            //        ["AppUrl"] = AppSettings.SiteLiveUrl
            //    });


            //// 15. Admin System Notification
            //await emailService.SendAsync(
            //    AppSettings.OwnerEmail,
            //    EmailTemplateCodes.AdminSystemNotification,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = AppSettings.OwnerEmail,
            //        ["NotificationTitle"] = "notificationTitle",
            //        ["NotificationMessage"] = "notificationMessage"
            //    });


            //// 16. Birthday Wish
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.BirthdayWish,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email
            //    });


            //// 17. Anniversary Wish
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.AnniversaryWish,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email
            //    });


            //// 18. Occasion Reminder
            //await emailService.SendAsync(
            //    email,
            //    EmailTemplateCodes.OccasionReminder,
            //    new Dictionary<string, string>
            //    {
            //        ["UserName"] = email,
            //        ["OccasionName"] = "occasionName",
            //        ["DateOfAction"] = DateTime.Now.ToString("dd MMM yyyy")
            //    });

            return Ok(new
            {
                registrationId,
                message = "Registration submitted successfully. Please check your email for the verification OTP."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
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
            var otpHash = EncryptDecryptAlgorithm.Encrypt(request.Otp.Trim());
            var verified = await registrationRepository.VerifyEmailAsync(request.RegistrationId, otpHash);

            if (!verified)
            {
                return BadRequest(new { error = "Invalid or expired OTP." });
            }
            var requesterDetails = await registrationRepository.GetDetailsAsync(request.RegistrationId);

            await emailService.SendAsync(
                requesterDetails.Email,
                EmailTemplateCodes.RegistrationSubmitted,
                new Dictionary<string, string>
                {
                    ["UserName"] = requesterDetails.Name,
                    ["Email"] = requesterDetails.Email,
                    ["DateOfAction"] = DateTime.Now.ToString("dd MMM yyyy hh:mm tt"),
                    ["AdminUrl"] = AppSettings.SiteLiveUrl
                });

            // 4. New Registration should sent to admin as notification for new user
            await emailService.SendAsync(
                AppSettings.OwnerEmail,
                EmailTemplateCodes.NewRegistrationAdmin,
                new Dictionary<string, string>
                {
                    ["UserName"] = requesterDetails.Name,
                    ["Email"] = requesterDetails.Email,
                    ["DateOfAction"] = DateTime.Now.ToString("dd MMM yyyy hh:mm tt"),
                    ["AdminUrl"] = AppSettings.SiteLiveUrl + "home/manage-users"
                });

            return Ok(new
            {
                success = true,
                message = "Email verified successfully. " + "Your registration is now pending administrator approval."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
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
            var userId = await registrationRepository.ApproveAsync(registrationId, CurrentUserId);

            if (!userId.HasValue)
            {
                return BadRequest(new
                {
                    error = "Unable to approve this registration. It may not exist or may already be processed."
                });
            }
            var requesterDetails = await registrationRepository.GetDetailsAsync(registrationId);

            userService.ForgotPassword(requesterDetails.Email);

            return Ok(new
            {
                success = true,
                message = "User approved successfully. and password sent to user registered email"
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
