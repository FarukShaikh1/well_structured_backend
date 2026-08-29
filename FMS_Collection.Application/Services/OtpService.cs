using FMS_Collection.Core.Common;
using FMS_Collection.Core.Constants;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using static System.Net.WebRequestMethods;

namespace FMS_Collection.Application.Services
{
    public class OtpService
    {
        private readonly IOtpRepository _otpRepository;
        private readonly INotificationSender _sender;
        private readonly IUserRepository _userRepository;
        private readonly IErrorRepository _errorRepository;
        private readonly IPasswordHasher _passwordHasher;

        public OtpService(IOtpRepository otpStore, INotificationSender sender, IUserRepository users, IErrorRepository errorRepository, IPasswordHasher passwordHasher)
        {
            _otpRepository = otpStore;
            _sender = sender;
            _userRepository = users;
            _errorRepository = errorRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ServiceResponse<bool>> StoreOtpAsync(SendEmailOtpRequest request, Guid? createdBy)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                var key = BuildKey(request.EmailId ?? string.Empty, request.Purpose);
                var expiresOn = DateTime.Now.AddMinutes(30);
                await _otpRepository.SetAsync(key, request.OtpCode, request.Purpose, expiresOn, createdBy);
            }, Constants.Messages.OtpStoredSuccessfully);
        }

        public async Task<ServiceResponse<bool>> SendMobileOtpAsync(SendMobileOtpRequest request, Guid? createdBy)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {

                var key = BuildKey(request.MobileNumber ?? string.Empty, request.Purpose);
                var expiresOn = DateTime.Now.AddMinutes(30);
                await _otpRepository.SetAsync(key, request.OtpCode, request.Purpose, expiresOn, createdBy);

                // ---------- SMS ----------
                if (!string.IsNullOrWhiteSpace(request.MobileNumber))
                {
                    var smsMessage = $"Your OTP is {request.OtpCode}. It is valid for 30 minutes.";
                    await _sender.SendSmsAsync(request.MobileNumber, smsMessage);
                }

                return true;
            }, Constants.Messages.OtpSentSuccessfully);
        }


        public async Task<ServiceResponse<bool>> SendWelcomeEmailAsync(string emailAddress, string planePassword)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                try
                {
                    // Get user by email
                    var user = await _userRepository.GetUserDetailsAsync(null, emailAddress);
                    if (user?.Id == null)
                        throw new Exception("User not found.");

                    string purpose = "Welcome new user";
                    // Build OTP key
                    var key = BuildKey(user.EmailAddress ?? "", purpose);

                    // OTP expiration (optional)
                    DateTime expiresOn = DateTime.Now.AddMinutes(24000);

                    // Save OTP
                    await _otpRepository.SetAsync(key, "", purpose, expiresOn, user.Id.Value);

                    // Email subject and body
                    var subject = "Welcome to FMS Collection 🎉";

                    var body = await _sender.GetTemplateAsync(
                    "WelcomeEmail.html",
                    new Dictionary<string, string>
                    {
                        { "FirstName", user.FirstName ?? "User" },
                        { "EmailAddress", user.EmailAddress ?? "your email address" },
                        { "Password", planePassword },
                        { "LoginUrl", AppSettings.SiteLiveUrl },
                        { "Year", DateTime.Now.Year.ToString() }
                    });

                    // Send email
                    if (!string.IsNullOrWhiteSpace(user.EmailAddress))
                        await _sender.SendEmailAsync(user.EmailAddress, subject, body);
                }
                catch (Exception ex)
                {
                    await _errorRepository.AddErrorLog(ex, "Error", Guid.Empty);
                }
                return true;

            }, Constants.Messages.WelcomeInviteSentSuccessfully);
        }


        public async Task<ServiceResponse<bool>> VerifyAsync(VerifyOtpRequest request)
        {
            var response = new ServiceResponse<bool>();
            var user = await _userRepository.GetUserDetailsAsync(null, request.EmailId);

            if (user == null) throw new Exception("User not found");
            var key = BuildKey(request.EmailId ?? string.Empty, request.Purpose);
            var entry = await _otpRepository.GetAsync(key);
            if (!entry.Exists || !string.Equals(entry.OtpCode, request.OtpCode, StringComparison.Ordinal))
            {
                response.Success = false;
                response.Message = Constants.Messages.OtpInvalidOrExpired;
                return response;
            }
            if (DateTime.Now > entry.ExpiresAt)
            {
                await _otpRepository.InvalidateAsync(key);
                response.Success = false;
                response.Message = Constants.Messages.OtpInvalidOrExpired;
                return response;
            }

            await _otpRepository.InvalidateAsync(key);
            response.Success = true;
            response.Data = true;
            response.Message = Constants.Messages.OtpVerifiedSuccessfully;
            return response;
        }

        public async Task<ServiceResponse<bool>> ResetPasswordWithOtpAsync(ResetPasswordWithOtpRequest request)
        {
            var user = await _userRepository.GetUserDetailsAsync(null, request.EmailId);

            // Verify OTP first
            var verify = await VerifyAsync(new VerifyOtpRequest
            {
                EmailId = request.EmailId,
                OtpCode = request.OtpCode,
                Purpose = Constants.OtpPurpose.PasswordReset
            });
            if (!verify.Success)
            {
                return verify;
            }

            // Find user and update password
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                var user = await _userRepository.GetUserDetailsAsync(null, request.EmailId);
                if (user == null) throw new Exception("User not found");

                // Hash password using PBKDF2-SHA512
                var newHash = _passwordHasher.Hash(request.NewPassword);
                await _userRepository.UpdatePasswordHashAsync(user.Id, newHash);

                return true;
            }, Constants.Messages.PasswordResetSuccessful);
        }

        private static string BuildKey(string emailId, string purpose)
        {
            return $"otp:{emailId.ToLowerInvariant()}:{purpose}";
        }
    }
}


