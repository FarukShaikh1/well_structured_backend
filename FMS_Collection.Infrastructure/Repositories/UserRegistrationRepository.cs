using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using FMS_Collection.Core.Response;
using FMS_Collection.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FMS_Collection.Infrastructure.Repositories
{
    public class UserRegistrationRepository
        : IUserRegistrationRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public UserRegistrationRepository(
            DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        // =========================
        // ADD REGISTRATION
        // =========================

        public async Task<Guid> AddAsync(
            UserRegistration registration)
        {
            using var conn = _dbFactory.CreateConnection();

            using var cmd = new SqlCommand("UserRegistration_Add", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@in_DateOfBirth", registration.DateOfBirth);
            cmd.Parameters.AddWithValue("@in_Name", registration.Name);
            cmd.Parameters.AddWithValue("@in_Email", registration.Email);
            cmd.Parameters.AddWithValue("@in_MobileNumber", (object?)registration.MobileNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_Address", (object?)registration.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_ProfilePhoto", (object?)registration.ProfilePhoto ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@in_OtpHash", registration.EmailVerificationOtp);
            //cmd.Parameters.AddWithValue("@in_OtpExpiry", registration.OtpExpiry);
            var outId = new SqlParameter("@out_RegistrationId", SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(outId);

            await conn.OpenAsync();
            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {

            }
            return (Guid)outId.Value;
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<UserRegistrationResponse?>
            GetByIdAsync(Guid registrationId)
        {
            using var conn =
                _dbFactory.CreateConnection();

            using var cmd =
                new SqlCommand(
                    "UserRegistration_GetById",
                    conn)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };

            cmd.Parameters.AddWithValue(
                "@in_RegistrationId",
                registrationId);

            await conn.OpenAsync();

            using var reader =
                await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapResponse(reader);
            }

            return null;
        }

        // =========================
        // GET BY EMAIL
        // =========================

        public async Task<UserRegistrationResponse?>
            GetByEmailAsync(string email)
        {
            using var conn =
                _dbFactory.CreateConnection();

            using var cmd = new SqlCommand("UserRegistration_GetByEmail", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@in_Email", email);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapResponse(reader);
            }
            return null;
        }

        // =========================
        // MAPPER
        // =========================

        private static UserRegistrationResponse
            MapResponse(SqlDataReader reader)
        {
            return new UserRegistrationResponse
            {
                Id =
                    reader.GetGuid(
                        reader.GetOrdinal("Id")),

                DateOfBirth =
                    reader.GetDateTime(
                        reader.GetOrdinal("DateOfBirth")),

                Name =
                    reader.GetString(
                        reader.GetOrdinal("Name")),

                Email =
                    reader.GetString(
                        reader.GetOrdinal("Email")),

                MobileNumber =
                    reader.IsDBNull(
                        reader.GetOrdinal("MobileNumber"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("MobileNumber")),

                Address =
                    reader.IsDBNull(
                        reader.GetOrdinal("Address"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("Address")),

                ProfilePhoto =
                    reader.IsDBNull(
                        reader.GetOrdinal("ProfilePhoto"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("ProfilePhoto")),

                EmailVerified =
                    reader.GetBoolean(
                        reader.GetOrdinal("EmailVerified")),

                Status =
                    reader.GetString(
                        reader.GetOrdinal("Status")),

                RejectionReason =
                    reader.IsDBNull(
                        reader.GetOrdinal("RejectionReason"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("RejectionReason")),

                ApprovedBy =
                    reader.IsDBNull(
                        reader.GetOrdinal("ApprovedBy"))
                        ? null
                        : reader.GetGuid(
                            reader.GetOrdinal("ApprovedBy")),

                ApprovedOn =
                    reader.IsDBNull(
                        reader.GetOrdinal("ApprovedOn"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal("ApprovedOn")),

                CreatedOn =
                    reader.GetDateTime(
                        reader.GetOrdinal("CreatedOn")),

                ModifiedOn =
                    reader.IsDBNull(
                        reader.GetOrdinal("ModifiedOn"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal("ModifiedOn"))
            };
        }


        // =========================================================
        // ADD REGISTRATION
        // =========================================================

        public async Task<Guid> AddAsync(
            UserRegistration registration,
            string otp)
        {
            using var conn = _dbFactory.CreateConnection();

            using var cmd = new SqlCommand(
                "UserRegistration_Add",
                conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue(
                "@in_DateOfBirth",
                registration.DateOfBirth);

            cmd.Parameters.AddWithValue(
                "@in_Name",
                registration.Name);

            cmd.Parameters.AddWithValue(
                "@in_Email",
                registration.Email);

            cmd.Parameters.AddWithValue(
                "@in_MobileNumber",
                (object?)registration.MobileNumber ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@in_Address",
                (object?)registration.Address ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@in_ProfilePhoto",
                (object?)registration.ProfilePhoto ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@in_Otp",
                otp);

            var outId = new SqlParameter(
                "@out_RegistrationId",
                SqlDbType.UniqueIdentifier)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(outId);

            await conn.OpenAsync();

            await cmd.ExecuteNonQueryAsync();

            return (Guid)outId.Value;
        }


        // =========================================================
        // GET DETAILS
        // =========================================================

        public async Task<UserRegistrationResponse?>
            GetDetailsAsync(Guid registrationId)
        {
            using var conn = _dbFactory.CreateConnection();

            using var cmd = new SqlCommand(
                "UserRegistration_Details_Get",
                conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue(
                "@in_RegistrationId",
                registrationId);

            await conn.OpenAsync();

            using var reader =
                await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapRegistrationResponse(reader);
            }

            return null;
        }


        // =========================================================
        // VERIFY EMAIL
        // =========================================================

        public async Task<bool> VerifyEmailAsync(
            Guid registrationId,
            string otp)
        {
            using var conn = _dbFactory.CreateConnection();

            using var cmd = new SqlCommand(
                "UserRegistration_VerifyEmail",
                conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue(
                "@in_RegistrationId",
                registrationId);

            cmd.Parameters.AddWithValue(
                "@in_Otp",
                otp);

            await conn.OpenAsync();

            var result =
                await cmd.ExecuteScalarAsync();

            return result != null &&
                   Convert.ToBoolean(result);
        }


        // =========================================================
        // EMAIL EXISTS IN USERS
        // =========================================================

        public async Task<bool> EmailExistsAsync(
            string email)
        {
            using var conn = _dbFactory.CreateConnection();

            using var cmd = new SqlCommand(
                "UserRegistration_UserEmailExists",
                conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue(
                "@in_Email",
                email);

            await conn.OpenAsync();

            var result =
                await cmd.ExecuteScalarAsync();

            return result != null &&
                   Convert.ToBoolean(result);
        }


        // =========================================================
        // PENDING REGISTRATION EXISTS
        // =========================================================

        public async Task<bool> PendingRegistrationExistsAsync(
            string email)
        {
            using var conn = _dbFactory.CreateConnection();

            using var cmd = new SqlCommand(
                "UserRegistration_PendingExists",
                conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue(
                "@in_Email",
                email);

            await conn.OpenAsync();

            var result =
                await cmd.ExecuteScalarAsync();

            return result != null &&
                   Convert.ToBoolean(result);
        }


        // =========================================================
        // GET PENDING APPROVAL
        // =========================================================

        public async Task<List<UserRegistrationResponse>>
            GetPendingApprovalAsync()
        {
            var result =
                new List<UserRegistrationResponse>();

            using var conn =
                _dbFactory.CreateConnection();

            using var cmd = new SqlCommand(
                "UserRegistration_PendingApproval_Get",
                conn)
            {
                CommandType =
                    CommandType.StoredProcedure
            };

            await conn.OpenAsync();

            using var reader =
                await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(
                    MapRegistrationResponse(reader));
            }

            return result;
        }


        // =========================================================
        // MAPPER
        // =========================================================

        private static UserRegistrationResponse
            MapRegistrationResponse(
                SqlDataReader reader)
        {
            return new UserRegistrationResponse
            {
                Id = reader.GetGuid(
                    reader.GetOrdinal("Id")),

                DateOfBirth = reader.GetDateTime(
                    reader.GetOrdinal("DateOfBirth")),

                Name = reader.GetString(
                    reader.GetOrdinal("Name")),

                Email = reader.GetString(
                    reader.GetOrdinal("Email")),

                MobileNumber =
                    reader.IsDBNull(
                        reader.GetOrdinal("MobileNumber"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("MobileNumber")),

                Address =
                    reader.IsDBNull(
                        reader.GetOrdinal("Address"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("Address")),

                ProfilePhoto =
                    reader.IsDBNull(
                        reader.GetOrdinal("ProfilePhoto"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal("ProfilePhoto")),

                EmailVerified =
                    reader.GetBoolean(
                        reader.GetOrdinal(
                            "EmailVerified")),

                Status = reader.GetString(
                    reader.GetOrdinal("Status")),

                RejectionReason =
                    reader.IsDBNull(
                        reader.GetOrdinal(
                            "RejectionReason"))
                        ? null
                        : reader.GetString(
                            reader.GetOrdinal(
                                "RejectionReason")),

                ApprovedBy =
                    reader.IsDBNull(
                        reader.GetOrdinal("ApprovedBy"))
                        ? null
                        : reader.GetGuid(
                            reader.GetOrdinal(
                                "ApprovedBy")),

                ApprovedOn =
                    reader.IsDBNull(
                        reader.GetOrdinal("ApprovedOn"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal(
                                "ApprovedOn")),

                CreatedOn =
                    reader.GetDateTime(
                        reader.GetOrdinal(
                            "CreatedOn")),

                ModifiedOn =
                    reader.IsDBNull(
                        reader.GetOrdinal(
                            "ModifiedOn"))
                        ? null
                        : reader.GetDateTime(
                            reader.GetOrdinal(
                                "ModifiedOn"))
            };
        }


        // =====================================================
        // GET PENDING
        // =====================================================

        public async Task<List<UserRegistrationResponse>>
            GetPendingAsync()
        {
            var result =
                new List<UserRegistrationResponse>();

            using var conn =
                _dbFactory.CreateConnection();

            using var cmd =
                new SqlCommand(
                    "UserRegistration_GetPending",
                    conn)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };

            await conn.OpenAsync();

            using var reader =
                await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(
                    MapUserRegistration(reader));
            }

            return result;
        }


        // =====================================================
        // APPROVE
        // =====================================================

        public async Task<bool> ApproveAsync(
            Guid registrationId,
            Guid approvedBy)
        {
            using var conn =
                _dbFactory.CreateConnection();

            using var cmd =
                new SqlCommand(
                    "UserRegistration_Approve",
                    conn)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };

            cmd.Parameters.AddWithValue(
                "@in_RegistrationId",
                registrationId);

            cmd.Parameters.AddWithValue(
                "@in_ApprovedBy",
                approvedBy);

            await conn.OpenAsync();
            try
            {
                var rowsAffected =
                    await cmd.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        // =====================================================
        // REJECT
        // =====================================================

        public async Task<bool> RejectAsync(
            Guid registrationId,
            string reason,
            Guid rejectedBy)
        {
            using var conn =
                _dbFactory.CreateConnection();

            using var cmd =
                new SqlCommand(
                    "UserRegistration_Reject",
                    conn)
                {
                    CommandType =
                        CommandType.StoredProcedure
                };

            cmd.Parameters.AddWithValue(
                "@in_RegistrationId",
                registrationId);

            cmd.Parameters.AddWithValue(
                "@in_RejectionReason",
                reason);

            cmd.Parameters.AddWithValue(
                "@in_RejectedBy",
                rejectedBy);

            await conn.OpenAsync();

            var rowsAffected =
                await cmd.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }


        // =====================================================
        // MAPPER
        // =====================================================

        private static UserRegistrationResponse
            MapUserRegistration(
                SqlDataReader reader)
        {
            return new UserRegistrationResponse
            {
                Id = reader.GetGuid(
                    reader.GetOrdinal("Id")),

                Name = reader.GetString(
                    reader.GetOrdinal("Name")),

                Email = reader.GetString(
                    reader.GetOrdinal("Email")),

                MobileNumber =
                    reader.IsDBNull(
                        reader.GetOrdinal("MobileNumber"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("MobileNumber")),

                Address =
                    reader.IsDBNull(
                        reader.GetOrdinal("Address"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("Address")),

                ProfilePhoto =
                    reader.IsDBNull(
                        reader.GetOrdinal("ProfilePhoto"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("ProfilePhoto")),

                DateOfBirth =
                    reader.GetDateTime(
                        reader.GetOrdinal("DateOfBirth")),

                EmailVerified =
                    reader.GetBoolean(
                        reader.GetOrdinal("EmailVerified")),

                Status =
                    reader.GetString(
                        reader.GetOrdinal("Status")),

                RejectionReason =
                    reader.IsDBNull(
                        reader.GetOrdinal("RejectionReason"))
                    ? null
                    : reader.GetString(
                        reader.GetOrdinal("RejectionReason")),

                CreatedOn =
                    reader.GetDateTime(
                        reader.GetOrdinal("CreatedOn")),

                ModifiedOn =
                    reader.IsDBNull(
                        reader.GetOrdinal("ModifiedOn"))
                    ? null
                    : reader.GetDateTime(
                        reader.GetOrdinal("ModifiedOn"))
            };
        }
    }
}
