using Dapper;
using PhotographyPortfolioAPI.Contracts.DTOs.Auth;
using PhotographyPortfolioAPI.Contracts.DTOs.MyProfile;
using PhotographyPortfolioAPI.Contracts.Models.Auth;
using PhotographyPortfolioAPI.Infrastructure.Base;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace PhotographyPortfolioAPI.Infrastructure.Repositories.Auth
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration config) : base(config) { }

        public async Task CreateAsync(User user)
        {
            var sql = @"INSERT INTO auth.users
                (user_id,user_name,email,mobile,first_name,last_name,password_hash,password_salt,is_active,user_type,designation_name,department_name)
                VALUES (@user_id,@user_name,@email,@mobile,@first_name,@last_name,@password_hash,@password_salt,@is_active,@user_type,@designation_name,@department_name)";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, new
            {
                user.user_id, user.user_name, user.email, user.mobile,
                user.first_name, user.last_name, user.password_hash, user.password_salt,
                user.is_active, user_type = user.user_type.ToString(),
                user.designation_name, user.department_name
            });
        }

        public async Task UpdateAsync(User user)
        {
            var sql = @"UPDATE auth.users
                    SET email=@email, mobile=@mobile, first_name=@first_name, last_name=@last_name,
                        is_active=@is_active, user_type=@user_type, designation_name=@designation_name, department_name=@department_name
                    WHERE user_id=@user_id";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, new
            {
                user.user_id, user.email, user.mobile, user.first_name, user.last_name,
                user.is_active, user_type = user.user_type.ToString(),
                user.designation_name, user.department_name
            });
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM auth.users WHERE user_name=@userName AND is_active=true", new { userName });
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM auth.users WHERE user_id=@userId", new { userId });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM auth.users WHERE email=@email AND is_active=true", new { email });
        }

        public async Task<UserMasterDto?> GetUserMasterDtoByUserIdAsync(Guid userId)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<UserMasterDto>(
                "SELECT * FROM auth.users WHERE user_id=@userId", new { userId });
        }

        public async Task UpdateUserRoleAsync(Guid userId, Guid roleId)
        {
            var sql = @"DELETE FROM auth.user_roles WHERE user_id = @userId;
                    INSERT INTO auth.user_roles(user_role_id,user_id,role_id)
                    VALUES (gen_random_uuid(),@userId,@roleId)";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, new { userId, roleId });
        }

        public async Task<Role?> GetUserRoleAsync(Guid userId)
        {
            var sql = @"SELECT r.* FROM auth.roles r
                    JOIN auth.user_roles ur ON r.role_id = ur.role_id
                    WHERE ur.user_id = @userId";
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Role>(sql, new { userId });
        }

        public async Task<List<UserPermissionsData>> GetPermissions(Guid userId)
        {
            var sql = @"SELECT p.permission_code,p.permission_name,p.module_name,
                    rp.role_permission_id,rp.permission_id,rp.role_id,
                    rp.can_view,rp.can_add,rp.can_update,rp.can_delete,rp.can_export
                    FROM auth.user_roles ur
                    JOIN auth.role_permissions rp ON ur.role_id = rp.role_id
                    JOIN auth.permissions p ON rp.permission_id = p.permission_id
                    WHERE ur.user_id = @userId AND p.is_assignable = true";
            using var con = CreateConnection();
            return (await con.QueryAsync<UserPermissionsData>(sql, new { userId })).ToList();
        }

        public async Task<List<UserPermissionsData>> GetAllPermissionForSuperAdmin()
        {
            var sql = @"SELECT p.permission_code, p.permission_name, p.module_name, p.permission_id,
                                TRUE AS can_view, TRUE AS can_add, TRUE AS can_update, TRUE AS can_delete, TRUE AS can_export
                            FROM auth.permissions p";
            using var con = CreateConnection();
            return (await con.QueryAsync<UserPermissionsData>(sql)).ToList();
        }

        public async Task<List<UserMasterWithRoleDto>> GetAllUsersAsync()
        {
            var sql = @"SELECT u.user_id, u.user_name, u.email, u.mobile, u.first_name, u.last_name,
                        (u.first_name || ' ' || u.last_name) AS full_name,
                        u.profile_img_path AS profile_img_url, u.profile_url, u.online_status, u.status_message,
                        u.designation_name, u.department_name, u.is_email_verified, u.is_mobile_verified,
                        u.is_active, u.user_type, r.role_name, r.role_code, u.profile_img_path, u.profile_banner_path
                    FROM auth.users u
                    LEFT JOIN auth.user_roles ur ON u.user_id = ur.user_id
                    LEFT JOIN auth.roles r ON ur.role_id = r.role_id
                    ORDER BY u.created_at DESC";
            using var con = CreateConnection();
            return (await con.QueryAsync<UserMasterWithRoleDto>(sql)).ToList();
        }

        public async Task<List<UserMasterDto>> GetAllActiveUsersAsync()
        {
            using var con = CreateConnection();
            return (await con.QueryAsync<UserMasterDto>("SELECT * FROM auth.users WHERE is_active=true")).ToList();
        }

        public async Task<List<UserMasterDto>> GetAllActiveUsersByUserTypeAsync(string user_type)
        {
            using var con = CreateConnection();
            return (await con.QueryAsync<UserMasterDto>(
                "SELECT * FROM auth.users WHERE is_active=true AND user_type=@user_type", new { user_type })).ToList();
        }

        public async Task ActiveInactiveAsync(ActiveInactiveRequest request)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "UPDATE auth.users SET is_active=@is_active WHERE user_id=@user_id",
                new { request.is_active, request.user_id });
        }

        public async Task<List<UserMasterDto>> GetUsersByIdsAsync(IEnumerable<Guid> userIds)
        {
            using var con = CreateConnection();
            return (await con.QueryAsync<UserMasterDto>(
                "SELECT * FROM auth.users WHERE user_id = ANY(@Ids)", new { Ids = userIds.ToArray() })).ToList();
        }

        public async Task<UserMasterDto?> GetProfileByIdAsync(Guid userId)
        {
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<UserMasterDto>(
                "SELECT * FROM auth.users WHERE user_id=@userId", new { userId });
        }

        public async Task<string?> ChangeProfileImageAsync(IFormFile? file, Guid userId, IWebHostEnvironment env)
        {
            var uploadsFolder = Path.Combine(env.WebRootPath ?? "wwwroot", "profile_images", userId.ToString());
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            using var con = CreateConnection();
            if (con.State == ConnectionState.Closed) con.Open();
            using var tx = con.BeginTransaction();
            try
            {
                var oldImage = await con.QueryFirstOrDefaultAsync<string>(
                    "SELECT profile_img_path FROM auth.users WHERE user_id = @UserId", new { UserId = userId }, tx);

                if (!string.IsNullOrEmpty(oldImage))
                {
                    var oldPath = Path.Combine(env.WebRootPath ?? "wwwroot", oldImage.TrimStart('/'));
                    if (File.Exists(oldPath)) File.Delete(oldPath);
                }

                if (file == null)
                {
                    await con.ExecuteAsync("UPDATE auth.users SET profile_img_path = NULL WHERE user_id = @UserId",
                        new { UserId = userId }, tx);
                    tx.Commit();
                    return null;
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension)) throw new ArgumentException("Only image files are allowed.");

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(stream);

                var dbPath = Path.Combine("profile_images", userId.ToString(), fileName).Replace("\\", "/");
                await con.ExecuteAsync("UPDATE auth.users SET profile_img_path = @profile_img_path WHERE user_id = @UserId",
                    new { profile_img_path = dbPath, UserId = userId }, tx);
                tx.Commit();
                return dbPath;
            }
            catch { tx.Rollback(); throw; }
        }

        public async Task<string?> ChangeProfilebannerAsync(IFormFile? file, Guid userId, IWebHostEnvironment env)
        {
            var uploadsFolder = Path.Combine(env.WebRootPath ?? "wwwroot", "profile_banners", userId.ToString());
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            using var con = CreateConnection();
            if (con.State == ConnectionState.Closed) con.Open();
            using var tx = con.BeginTransaction();
            try
            {
                var oldImage = await con.QueryFirstOrDefaultAsync<string>(
                    "SELECT profile_banner_path FROM auth.users WHERE user_id = @UserId", new { UserId = userId }, tx);

                if (!string.IsNullOrEmpty(oldImage))
                {
                    var oldPath = Path.Combine(env.WebRootPath ?? "wwwroot", oldImage.TrimStart('/'));
                    if (File.Exists(oldPath)) File.Delete(oldPath);
                }

                if (file == null)
                {
                    await con.ExecuteAsync("UPDATE auth.users SET profile_banner_path = NULL WHERE user_id = @UserId",
                        new { UserId = userId }, tx);
                    tx.Commit();
                    return null;
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension)) throw new ArgumentException("Only image files are allowed.");

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(stream);

                var dbPath = Path.Combine("profile_banners", userId.ToString(), fileName).Replace("\\", "/");
                await con.ExecuteAsync("UPDATE auth.users SET profile_banner_path = @profile_banner_path WHERE user_id = @UserId",
                    new { profile_banner_path = dbPath, UserId = userId }, tx);
                tx.Commit();
                return dbPath;
            }
            catch { tx.Rollback(); throw; }
        }

        public async Task<UserMasterDto?> UpdateProfileAsync(UserMasterDto user)
        {
            const string sql = @"UPDATE auth.users
                SET email=@email, mobile=@mobile, first_name=@first_name, last_name=@last_name, profile_url=@profile_url
                WHERE user_id=@user_id";
            using var con = CreateConnection();
            var affected = await con.ExecuteAsync(sql, user);
            return affected > 0 ? user : null;
        }

        public async Task ChangePasswordAsync(string hash, string salt, Guid userId)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "UPDATE auth.users SET password_hash=@password_hash, password_salt=@password_salt WHERE user_id=@user_id",
                new { password_hash = hash, password_salt = salt, user_id = userId });
        }

        public async Task UpdateOnlineStatusAsync(UpdateOnlineStatus request, Guid userId)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "UPDATE auth.users SET online_status=@online_status WHERE user_id=@user_id",
                new { online_status = request.online_status, user_id = userId });
        }

        public async Task UpdateStatusMessageAsync(UpdateStatusMessage request, Guid userId)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "UPDATE auth.users SET status_message=@status_message WHERE user_id=@user_id",
                new { status_message = request.status_message, user_id = userId });
        }

        public async Task CreateOtpAsync(UserOtp otp)
        {
            var sql = @"INSERT INTO auth.user_otps (user_id, otp_code, otp_type, expires_at)
                        VALUES (@user_id, @otp_code, @otp_type, @expires_at)";
            using var con = CreateConnection();
            await con.ExecuteAsync(sql, new { otp.user_id, otp.otp_code, otp.otp_type, otp.expires_at });
        }

        public async Task<UserOtp?> GetActiveOtpAsync(Guid userId, string otpCode, string otpType)
        {
            var sql = @"SELECT * FROM auth.user_otps
                        WHERE user_id=@userId AND otp_code=@otpCode AND otp_type=@otpType
                          AND is_used=false AND expires_at > NOW()
                        ORDER BY created_at DESC LIMIT 1";
            using var con = CreateConnection();
            return await con.QueryFirstOrDefaultAsync<UserOtp>(sql, new { userId, otpCode, otpType });
        }

        public async Task MarkOtpUsedAsync(long otpId)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync("UPDATE auth.user_otps SET is_used=true WHERE otp_id=@otpId", new { otpId });
        }

        public async Task UpdateEmailVerifiedAsync(Guid userId)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync("UPDATE auth.users SET is_email_verified=true WHERE user_id=@userId", new { userId });
        }

        public async Task UpdatePasswordAsync(Guid userId, string passwordHash, string passwordSalt)
        {
            using var con = CreateConnection();
            await con.ExecuteAsync(
                "UPDATE auth.users SET password_hash=@passwordHash, password_salt=@passwordSalt WHERE user_id=@userId",
                new { userId, passwordHash, passwordSalt });
        }

        public async Task<List<UserMasterDto>> GetPendingClientsAsync()
        {
            var sql = @"SELECT user_id, user_name, email, mobile, first_name, last_name,
                               (first_name || ' ' || last_name) AS full_name, is_active, user_type
                        FROM auth.users
                        WHERE user_type = 'CLIENT'
                          AND user_id NOT IN (SELECT client_id FROM client_master)
                        ORDER BY created_at DESC";
            using var con = CreateConnection();
            return (await con.QueryAsync<UserMasterDto>(sql)).ToList();
        }
    }
}
