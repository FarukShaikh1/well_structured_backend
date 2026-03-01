using FMS_Collection.Core.Common;
using FMS_Collection.Core.Constants;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;

namespace FMS_Collection.Application.Services
{
    public class OtpService
    {
        private readonly IOtpRepository _otpRepository;
        private readonly INotificationSender _sender;
        private readonly IUserRepository _userRepository;
        private readonly IErrorRepository _errorRepository;

        public OtpService(IOtpRepository otpStore, INotificationSender sender, IUserRepository users, IErrorRepository errorRepository)
        {
            _otpRepository = otpStore;
            _sender = sender;
            _userRepository = users;
            _errorRepository = errorRepository;
        }

        public async Task<ServiceResponse<bool>> SendAsync(SendOtpRequest request)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                // Resolve user by input (username/email/phone) using UserRepository methods
                var user = await _userRepository.GetUserDetailsAsync(null, request.EmailId);

                if (user.Id == null) throw new Exception("User not found");

                var otp = RandomGeneratorService.GenerateNumericOtp(6);
                var key = BuildKey(user.Id, user.EmailAddress ?? string.Empty, request.Purpose);
                var expiresOn = DateTime.Now.AddMinutes(30);
                await _otpRepository.SetAsync(key, otp, request.Purpose, expiresOn, user.Id);

                // ---------- EMAIL ----------
                if (!string.IsNullOrWhiteSpace(user.EmailAddress))
                {
                    var templateValues = new Dictionary<string, string>
                    {
                        { "EmailAddress", user.EmailAddress ?? "Your Email Address" },
                        { "UserName", user.FirstName ?? "User" },
                        { "Otp", otp },
                        { "Purpose", request.Purpose },
                        { "AppUrl", AppSettings.SiteLiveUrl },
                        { "ExpiryMinutes", "30" },
                        { "Year", DateTime.Now.Year.ToString() }
                    };
                    string subject = "Your OTP From FMS Collection";

                    var htmlBody = await _sender.GetTemplateAsync(
                        "OtpEmail.html",
                        templateValues
                    );

                    await _sender.SendEmailAsync(
                        user.EmailAddress,
                        subject,
                        htmlBody,
                        isBodyHtml: true
                    );
                }

                // ---------- SMS ----------
                if (!string.IsNullOrWhiteSpace(user.MobileNumber))
                {
                    var smsMessage = $"Your OTP is {otp}. It is valid for 30 minutes.";
                    await _sender.SendSmsAsync(user.MobileNumber, smsMessage);
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
                    var key = BuildKey(user.Id.Value, user.EmailAddress ?? "", purpose);

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
            var key = BuildKey(user.Id, user.EmailAddress ?? string.Empty, request.Purpose);
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

                // Hash password using existing algorithm
                var hasher = new Core.Common.HashAlgorithm();
                var newHash = hasher.GetHash(request.NewPassword);
                _userRepository.UpdatePasswordHashAsync(user.Id, newHash);

                return true;
            }, Constants.Messages.PasswordResetSuccessful);
        }

        private static string BuildKey(Guid? userId, string emailId, string purpose)
        {
            return $"otp:{userId}:{purpose}:{emailId.ToLowerInvariant()}";
        }
    }
}


