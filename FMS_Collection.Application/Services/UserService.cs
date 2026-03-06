// Application/Services/UserService.cs
using FMS_Collection.Core.Common;
using FMS_Collection.Core.Constants;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Request;
using FMS_Collection.Core.Response;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FMS_Collection.Application.Services
{
    public class UserService(
        IUserRepository repository,
        IRefreshTokenRepository refreshTokenRepository,
        IPermissionRepository permissionRepository,
        ITokenService tokenService,
        IPasswordHasher passwordHasher,
        IAuditService auditService,
        OtpService otpService,
        AzureBlobService blobService,
        IMemoryCache cache,
        ILogger<UserService> logger)
    {
        // ── Auth ──────────────────────────────────────────────────────────────

        public async Task<ServiceResponse<AuthResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var loginData = await repository.GetLoginDetails(request);

                if (loginData?.Id == null)
                    return ServiceResponse<AuthResponse>.Fail(Constants.Messages.InvalidUserOrPassword, 401);

                if (loginData.IsDeleted)
                    return ServiceResponse<AuthResponse>.Fail(Constants.Messages.InvalidUserOrPassword, 401);

                if (loginData.IsLocked)
                    return ServiceResponse<AuthResponse>.Fail(Constants.Messages.TooManyLoginAttempts, 403);

                if (loginData.FailedLoginCount > AppSettings.AllowedFailedLoginCount)
                    return ServiceResponse<AuthResponse>.Fail(Constants.Messages.TooManyLoginAttempts, 403);

                // Verify password — PBKDF2 first, fall back to legacy SHA1
                bool passwordValid = loginData.Password != null && (
                    passwordHasher.Verify(request.Password, loginData.Password) ||
                    passwordHasher.VerifyLegacySha1(request.Password, loginData.Password));

                if (!passwordValid)
                    return ServiceResponse<AuthResponse>.Fail(Constants.Messages.InvalidUserOrPassword, 401);

                // Silently upgrade legacy SHA1 hash to PBKDF2
                if (loginData.Password != null && passwordHasher.VerifyLegacySha1(request.Password, loginData.Password))
                {
                    var newHash = passwordHasher.Hash(request.Password);
                    await repository.UpdatePasswordHashAsync(loginData.Id, newHash);
                    logger.LogInformation("Upgraded legacy password hash for user {UserId}", loginData.Id);
                }

                // OTP flow — don't issue tokens yet
                if (loginData.IsOtpRequired)
                {
                    await otpService.SendAsync(new SendOtpRequest
                    {
                        EmailId = loginData.EmailAddress!,
                        Purpose = Constants.OtpPurpose.Login
                    });

                    return ServiceResponse<AuthResponse>.Ok(
                        new AuthResponse { IsOtpRequired = true, UserId = loginData.Id!.Value },
                        "OTP sent to your registered email.");
                }

                var authResponse = await BuildAuthResponseAsync(loginData);
                await auditService.LogAsync(loginData.Id, "Login", "User", loginData.Id.ToString());

                return ServiceResponse<AuthResponse>.Ok(authResponse, Constants.Messages.LoginSuccessful);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Login failed for user {UserName}", request.UserName);
                return ServiceResponse<AuthResponse>.Fail("An error occurred while processing your request.", 500);
            }
        }

        public async Task<ServiceResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                var storedToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

                if (storedToken == null || !storedToken.IsActive)
                    return ServiceResponse<AuthResponse>.Fail("Invalid or expired refresh token.", 401);

                await refreshTokenRepository.RevokeAsync(request.RefreshToken);

                var loginData = await repository.GetUserLoginDataAsync(storedToken.UserId);
                if (loginData == null)
                    return ServiceResponse<AuthResponse>.Fail("User not found.", 401);

                var authResponse = await BuildAuthResponseAsync(loginData);
                return ServiceResponse<AuthResponse>.Ok(authResponse, "Token refreshed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Refresh token failed");
                return ServiceResponse<AuthResponse>.Fail("An error occurred while processing your request.", 500);
            }
        }

        public async Task LogoutAsync(string refreshToken, Guid userId)
        {
            try
            {
                await refreshTokenRepository.RevokeAsync(refreshToken);
                await auditService.LogAsync(userId, "Logout", "User", userId.ToString());
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Logout cleanup failed for user {UserId}", userId);
            }
        }

        // ── User CRUD ─────────────────────────────────────────────────────────

        public async Task<ServiceResponse<List<UserListResponse>>> GetUserListAsync(Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => repository.GetUserListAsync(userId),
                Constants.Messages.UserListFetchedSuccessfully,
                logger);
        }

        public async Task<ServiceResponse<UserDetailsResponse>> GetUserDetailsAsync(Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => repository.GetUserDetailsAsync(userId),
                Constants.Messages.UserDetailsFetchedSuccessfully,
                logger);
        }

        // Overload used by SpecialOccasionService (lookup by email)
        public async Task<ServiceResponse<UserDetailsResponse>> GetUserDetailsAsync(Guid? userId, string? emailId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => repository.GetUserDetailsAsync(userId, emailId),
                Constants.Messages.UserDetailsFetchedSuccessfully,
                logger);
        }

        public async Task<Guid> AddUserAsync(UserRequest user, Guid createdBy)
        {
            string plainPassword = user.Password ?? throw new ArgumentException("Password is required.");
            user.Password = passwordHasher.Hash(plainPassword);

            var id = await repository.AddAsync(user, createdBy);
            if (id != Guid.Empty)
                await otpService.SendWelcomeEmailAsync(user.EmailAddress, plainPassword);

            await auditService.LogAsync(createdBy, "UserCreated", "User", id.ToString(),
                newValues: new { user.EmailAddress});

            return id;
        }

        public async Task UpdateUserAsync(UserRequest user, Guid updatedBy)
        {
            await repository.UpdateAsync(user, updatedBy);
            await auditService.LogAsync(updatedBy, "UserUpdated", "User");
        }

        public async Task<ServiceResponse<bool>> DeleteUserAsync(Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => repository.DeleteAsync(userId),
                Constants.Messages.UserDeletedSuccessfully,
                logger);
        }

        public async Task<ServiceResponse<bool>> UpdateUserPermissionAsync(UserPermissionRequest userPermission, Guid updatedBy)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => repository.UpdateUserPermissionAsync(userPermission, updatedBy),
                Constants.Messages.UserPermissionsUpdatedSuccessfully,
                logger);
        }

        public async Task<ServiceResponse<(bool IsSuccess, string Message)>> ChangePassword(
            string oldPassword, string newPassword, Guid? userId, Guid? modifiedBy)
        {
            var loginData = await repository.GetUserLoginDataAsync(userId!.Value);
            if (loginData == null)
                return ServiceResponse<(bool, string)>.Fail("User not found.", 404);

            bool oldValid = loginData.Password != null && (
                passwordHasher.Verify(oldPassword, loginData.Password) ||
                passwordHasher.VerifyLegacySha1(oldPassword, loginData.Password));

            if (!oldValid)
                return ServiceResponse<(bool, string)>.Ok(
                    (false, "Current password is incorrect."),
                    "Change password attempted.");

            string newHash = passwordHasher.Hash(newPassword);
            await repository.UpdatePasswordHashAsync(userId, newHash);
            await auditService.LogAsync(modifiedBy, "PasswordChanged", "User", userId.ToString());

            return ServiceResponse<(bool, string)>.Ok(
                (true, "Password changed successfully."),
                Constants.Messages.UserPermissionsUpdatedSuccessfully);
        }

        // ── Module list (cached) ───────────────────────────────────────────────

        public async Task<ServiceResponse<List<ModuleListResponse>>> GetModuleListAsync()
        {
            const string cacheKey = "ModuleList";

            if (!cache.TryGetValue(cacheKey, out List<ModuleListResponse>? modules))
            {
                modules = await repository.GetModuleListAsync();
                cache.Set(cacheKey, modules, TimeSpan.FromHours(6));
            }

            return ServiceResponse<List<ModuleListResponse>>.Ok(
                modules!, Constants.Messages.ModulesFetchedSuccessfully);
        }

        public async Task<ServiceResponse<List<UserPermissionResponse>>> GetUserPermissionListAsync(Guid userId)
        {
            return await ServiceExecutor.ExecuteAsync(
                () => repository.GetUserPermissionListAsync(userId),
                Constants.Messages.UserPermissionsFetchedSuccessfully,
                logger);
        }

        // ── Private helpers ────────────────────────────────────────────────────

        private async Task<AuthResponse> BuildAuthResponseAsync(LoginResponse loginData)
        {
            Guid userId = loginData.Id!.Value;

            var permissions = await permissionRepository.GetPermissionNamesByRoleAsync(loginData.RoleId ?? Guid.Empty);

            string accessToken = tokenService.GenerateAccessToken(
                userId,
                loginData.EmailAddress ?? string.Empty,
                loginData.UserName ?? string.Empty,
                loginData.RoleId,
                loginData.RoleName,
                isSuperAdmin: loginData.IsSuperAdmin,
                permissions);

            string rawRefreshToken = tokenService.GenerateRefreshToken();

            await refreshTokenRepository.AddAsync(new RefreshToken
            {
                Token = rawRefreshToken,
                UserId = userId,
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(7)
            });

            string? imageSasUrl = !string.IsNullOrEmpty(loginData.ImagePath)
                ? blobService.GetBlobSasUrl(loginData.ImagePath) : null;

            string? thumbSasUrl = !string.IsNullOrEmpty(loginData.ThumbnailPath)
                ? blobService.GetBlobSasUrl(loginData.ThumbnailPath) : null;

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = rawRefreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(15),
                UserId = userId,
                FirstName = loginData.FirstName,
                LastName = loginData.LastName,
                UserName = loginData.UserName,
                EmailAddress = loginData.EmailAddress,
                RoleName = loginData.RoleName,
                RoleId = loginData.RoleId,
                IsSuperAdmin = loginData.IsSuperAdmin,
                ImagePathSasUrl = imageSasUrl,
                ThumbnailPathSasUrl = thumbSasUrl,
                IsOtpRequired = false,
                SpecialOccasionDate = loginData.SpecialOccasionDate
            };
        }
    }
}
