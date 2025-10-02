using FMS_Collection.Core.Common;
using FMS_Collection.Core.Constants;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using System.Security.Cryptography;
using System.Text;

namespace FMS_Collection.Application.Services
{
    public class OtpService
    {
        private readonly IOtpRepository _otpRepository;
        private readonly INotificationSender _sender;
        private readonly IUserRepository _userRepository;

        public OtpService(IOtpRepository otpStore, INotificationSender sender, IUserRepository users)
        {
            _otpRepository = otpStore;
            _sender = sender;
            _userRepository = users;
        }

        public async Task<ServiceResponse<bool>> SendAsync(SendOtpRequest request)
        {
            return await ServiceExecutor.ExecuteAsync(async () =>
            {
                // Resolve user by input (username/email/phone) using UserRepository methods
                var user= await _userRepository.GetUserDetailsAsync(null, request.EmailId);                

                if (user.Id == null) throw new Exception("User not found");

                var otp = GenerateNumericOtp(6);
                var key = BuildKey(user.Id, user.EmailAddress ?? string.Empty, request.Purpose);
                var expiresOn = DateTime.Now.AddMinutes(30);
                await _otpRepository.SetAsync(key, otp, request.Purpose, expiresOn, user.Id);

                var subject = "Your OTP Code";
                var message = $"Your OTP is {otp}. It will expire in 30 minutes.";
                if (!string.IsNullOrWhiteSpace(user.EmailAddress))
                    await _sender.SendEmailAsync(user.EmailAddress, subject, message);
                if (!string.IsNullOrWhiteSpace(user.MobileNumber))
                    await _sender.SendSmsAsync(user.MobileNumber, message);

                return true;
            }, Constants.Messages.OtpSentSuccessfully);
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

        private static string GenerateNumericOtp(int length)
        {
            const string digits = "0123456789";
            var data = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(digits[data[i] % digits.Length]);
            }
            return sb.ToString();
        }
    }
}


