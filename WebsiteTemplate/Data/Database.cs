using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using SystemPlus;
using SystemPlus.Data;
using WebsiteTemplate.Models;

namespace WebsiteTemplate.Data
{
    /// <summary>
    /// Database access layer
    /// </summary>
    public class Database : SqlServerBase
    {
        public Database(string connectionString) : base(connectionString)
        {

        }

        public static string MakeConnectionString()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = "DatabaseAddress",
                InitialCatalog = "DatabaseName",
                UserID = "DatabaseUserId",
                Password = "DatabasePassword",
                IntegratedSecurity = false,
                PersistSecurityInfo = true,
                Pooling = true,
                MultipleActiveResultSets = true,
                ConnectTimeout = 30
            };

            return sqlBuilder.ConnectionString;
        }

        #region Users

        public async virtual Task AddUserAsync(User user, CancellationToken token)
        {
            string apiKey = Guid.NewGuid().ToString();

            if (user.Config == null)
                user.Config = UserConfig.DefaultConfig;

            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("AddUser"))
            {
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@normalizedEmail", user.NormalizedEmail);
                cmd.Parameters.AddWithValue("@passwordHash", user.PasswordHash, null);
                cmd.Parameters.AddWithValue("@securityStamp", user.SecurityStamp, null);
                cmd.Parameters.AddWithValue("@twoFactorEnabled", user.TwoFactorEnabled);
                cmd.Parameters.AddWithValue("@userName", user.UserName, null);
                cmd.Parameters.AddWithValue("@normalizedUserName", user.NormalizedUserName, null);
                cmd.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber, null);
                cmd.Parameters.AddWithValue("@phoneNumberConfirmed", user.PhoneNumberConfirmed);
                cmd.Parameters.AddWithValue("@lockoutEnabled", user.LockoutEnabled);
                cmd.Parameters.AddWithValue("@apiKey", apiKey);
                cmd.Parameters.AddJsonValue("@options", user.Config);

                object o = await cmd.ExecuteScalarAsync(token);
                user.Id = Convert.ToInt32(o);
            }
        }

        public async virtual Task UpdateUserAsync(User user, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("UpdateUser"))
            {
                cmd.Parameters.AddWithValue("@userId", user.Id);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@normalizedEmail", user.NormalizedEmail);
                cmd.Parameters.AddWithValue("@emailConfirmed", user.EmailConfirmed);
                cmd.Parameters.AddWithValue("@passwordHash", user.PasswordHash, null);
                cmd.Parameters.AddWithValue("@securityStamp", user.SecurityStamp, null);
                cmd.Parameters.AddWithValue("@twoFactorEnabled", user.TwoFactorEnabled);
                cmd.Parameters.AddWithValue("@userName", user.UserName, null);
                cmd.Parameters.AddWithValue("@normalizedUserName", user.NormalizedUserName, null);
                cmd.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber, null);
                cmd.Parameters.AddWithValue("@phoneNumberConfirmed", user.PhoneNumberConfirmed);
                cmd.Parameters.AddWithValue("@lockoutEnd", user.LockoutEnd, null);
                cmd.Parameters.AddWithValue("@lockoutEnabled", user.LockoutEnabled);
                cmd.Parameters.AddWithValue("@accessFailedCount", user.AccessFailedCount);

                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async virtual Task UpdateUserConfigAsync(User user, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("UpdateUserOptions"))
            {
                cmd.Parameters.AddWithValue("@userId", user.Id);
                cmd.Parameters.AddJsonValue("@options", user.Config);

                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async virtual Task<User> GetUserAsync(int userId, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetUser"))
            {
                cmd.Parameters.AddWithValue("@userId", userId);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.TryReadItem(ReadUser);
                }
            }
        }

        public async Task<User> GetUserByEmailAsync(string normalizedEmail, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetUserByEmail"))
            {
                cmd.Parameters.AddWithValue("@normalizedEmail", normalizedEmail);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.TryReadItem(ReadUser);
                }
            }
        }

        public async Task<User> GetUserByUserNameAsync(string normalizedUserName, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetUserByUserName"))
            {
                cmd.Parameters.AddWithValue("@normalizedUserName", normalizedUserName);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.TryReadItem(ReadUser);
                }
            }
        }

        public async virtual Task DeleteUserAsync(int userId, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("DeleteUser"))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        private User ReadUser(IDataReader rdr)
        {
            int id = rdr.GetValue<int>("Id");
            DateTime timestamp = rdr.GetValue<DateTime>("Timestamp");
            string userName = rdr.GetValue<string>("UserName");
            string normalizedUserName = rdr.GetValue<string>("NormalizedUserName");
            string email = rdr.GetValue<string>("Email");
            string normalizedEmail = rdr.GetValue<string>("NormalizedEmail");
            bool emailConfirmed = rdr.GetValue<bool>("EmailConfirmed");
            string phoneNumber = rdr.GetValue<string>("PhoneNumber");
            bool phoneNumberConfirmed = rdr.GetValue<bool>("PhoneNumberConfirmed");
            string passwordHash = rdr.GetValue<string>("PasswordHash");
            string securityStamp = rdr.GetValue<string>("SecurityStamp");
            bool twoFactorEnabled = rdr.GetValue<bool>("TwoFactorEnabled");
            DateTimeOffset? lockoutEnd = rdr.GetValue<DateTimeOffset?>("LockoutEnd");
            bool lockoutEnabled = rdr.GetValue<bool>("LockoutEnabled");
            int accessFailedCount = rdr.GetValue<int>("AccessFailedCount");

            UserConfig config = rdr.GetJsonValue<UserConfig>("Options");

            if (config == null)
                config = UserConfig.DefaultConfig;

            if (config.Currency == null)
                config.Currency = "GBP";

            User user = new User()
            {
                Id = id,
                Timestamp = timestamp,
                UserName = userName,
                NormalizedUserName = normalizedUserName,
                Email = email,
                NormalizedEmail = normalizedEmail,
                EmailConfirmed = emailConfirmed,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = phoneNumberConfirmed,
                PasswordHash = passwordHash,
                SecurityStamp = securityStamp,
                TwoFactorEnabled = twoFactorEnabled,
                LockoutEnd = lockoutEnd,
                LockoutEnabled = lockoutEnabled,
                AccessFailedCount = accessFailedCount,

                Config = config,
            };

            return user;
        }

        #endregion

        #region LoginAttempts

        public async Task AddLoginAttmptAsync(LoginAttempt login, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("AddLoginAttempt"))
            {
                cmd.Parameters.AddWithValue("@userId", login.UserId);
                cmd.Parameters.AddWithValue("@ipAddress", login.IpAddress ?? string.Empty);
                cmd.Parameters.AddWithValue("@userAgent", login.UserAgent, null);
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async Task<IList<LoginAttempt>> GetLoginAttemptsAsync(int userId, int count, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetLoginAttempts"))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@count", count);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.ReadAll(ReadLoginAttempt);
                }
            }
        }

        private LoginAttempt ReadLoginAttempt(IDataReader rdr)
        {
            int id = rdr.GetValue<int>("Id");
            int uId = rdr.GetValue<int>("UserId");
            DateTime timestamp = rdr.GetValue<DateTime>("Timestamp");
            string ipAddress = rdr.GetValue<string>("IpAddress");
            string userAgent = rdr.GetValue<string>("UserAgent");

            return new LoginAttempt()
            {
                Id = id,
                UserId = uId,
                Timestamp = timestamp,
                IpAddress = ipAddress,
                UserAgent = userAgent,
            };
        }

        #endregion

        #region UserTokens

        public async Task SetUserTokenAsync(User user, string loginProvider, string name, string value, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("SetUserToken"))
            {
                cmd.Parameters.AddWithValue("@userId", user.Id);
                cmd.Parameters.AddWithValue("@loginProvider", loginProvider);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@value", value);
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async Task<string> GetUserTokenAsync(User user, string loginProvider, string name, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetUserToken"))
            {
                cmd.Parameters.AddWithValue("@userId", user.Id);
                cmd.Parameters.AddWithValue("@loginProvider", loginProvider);
                cmd.Parameters.AddWithValue("@name", name);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    if (!await rdr.ReadAsync(token))
                        return null;

                    return rdr.GetValue<string>("Value");
                }
            }
        }

        public async Task DeleteUserTokenAsync(int userId, string loginProvider, string name, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("DeleteUserToken"))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@loginProvider", loginProvider);
                cmd.Parameters.AddWithValue("@name", name);

                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public UserToken ReadUserToken(IDataReader rdr)
        {
            UserToken userToken = new UserToken
            {
                UserId = rdr.GetValue<int>("UserId"),
                LoginProvider = rdr.GetValue<string>("LoginProvider"),
                Name = rdr.GetValue<string>("Name"),
                Value = rdr.GetValue<string>("Value"),
            };
            return userToken;
        }

        #endregion

        #region UserLogins

        public async Task AddUserLoginAsync(UserLogin userLogin, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("AddUserLogin"))
            {
                cmd.Parameters.AddWithValue("@loginProvider", userLogin.LoginProvider);
                cmd.Parameters.AddWithValue("@providerKey", userLogin.ProviderKey);
                cmd.Parameters.AddWithValue("@userId", userLogin.UserId);
                cmd.Parameters.AddWithValue("@providerDisplayName", userLogin.ProviderDisplayName, null);

                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async Task<UserLogin> GetUserLoginAsync(string loginProvider, string providerKey, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetUserLogin"))
            {
                cmd.Parameters.AddWithValue("@loginProvider", loginProvider);
                cmd.Parameters.AddWithValue("@providerKey", providerKey);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.TryReadItem(ReadUserLogin);
                }
            }
        }

        public async Task<IList<UserLogin>> GetUserLoginsAsync(int userId, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetUserLogins"))
            {
                cmd.Parameters.AddWithValue("@userId", userId);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.ReadAll(ReadUserLogin);
                }
            }
        }

        public async Task DeleteUserLoginAsync(int userId, string loginProvider, string providerKey, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("DeleteUserLogin"))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@loginProvider", loginProvider);
                cmd.Parameters.AddWithValue("@providerKey", providerKey);
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        private UserLogin ReadUserLogin(IDataReader rdr)
        {
            UserLogin userLogin = new UserLogin
            {
                LoginProvider = rdr.GetValue<string>("LoginProvider"),
                ProviderKey = rdr.GetValue<string>("ProviderKey"),
                UserId = rdr.GetValue<int>("UserId"),
                ProviderDisplayName = rdr.GetValue<string>("ProviderDisplayName"),
            };

            return userLogin;
        }

        #endregion

        #region Roles

        public async Task AddRoleAsync(Role role, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("AddRole"))
            {
                cmd.Parameters.AddWithValue("@name", role.Name);
                cmd.Parameters.AddWithValue("@nameNormalized", role.NormalizedName);

                object o = await cmd.ExecuteScalarAsync(token);
                role.Id = Convert.ToInt32(o);
            }
        }

        public async Task UpdateRoleAsync(Role role, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("UpdateRole"))
            {
                cmd.Parameters.AddWithValue("@roleId", role.Id);
                cmd.Parameters.AddWithValue("@name", role.Name);
                cmd.Parameters.AddWithValue("@nameNormalized", role.NormalizedName);
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async Task<Role> GetRoleAsync(int id, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetRole"))
            {
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.TryReadItem(ReadRole);
                }
            }
        }

        public async Task<Role> GetRoleByNameAsync(string normalizedName, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetRoleByName"))
            {
                cmd.Parameters.AddWithValue("@normalizedName", normalizedName);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.TryReadItem(ReadRole);
                }
            }
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("AddToRole"))
            {
                cmd.Parameters.AddWithValue("@userId", user.Id);
                cmd.Parameters.AddWithValue("@roleName", roleName);
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("RemoveFromRole"))
            {
                cmd.Parameters.AddWithValue("@userId", user.Id);
                cmd.Parameters.AddWithValue("@roleName", roleName);
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async Task DeleteRoleAsync(int id, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("DeleteRole"))
            {
                cmd.Parameters.AddWithValue("@id", id);
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async Task<IList<Role>> GetRolesAsync(User user, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetRoles"))
            {
                cmd.Parameters.AddWithValue("@userId", user.Id);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.ReadAll(ReadRole);
                }
            }
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("IsInRole"))
            {
                cmd.Parameters.AddWithValue("@userId", user.Id);
                cmd.Parameters.AddWithValue("@roleName", roleName);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    if (!await rdr.ReadAsync(token))
                        return false;

                    return rdr.GetValue<int>("result") == 1;
                }
            }
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetUsersInRole"))
            {
                cmd.Parameters.AddWithValue("@roleName", roleName);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.ReadAll(ReadUser);
                }
            }
        }

        private Role ReadRole(IDataReader rdr)
        {
            int id = rdr.GetValue<int>("Id");
            DateTime timestamp = rdr.GetValue<DateTime>("Timestamp");
            string name = rdr.GetValue<string>("Name");
            string nameNormalized = rdr.GetValue<string>("NormalizedName");

            return new Role()
            {
                Id = id,
                Timestamp = timestamp,
                Name = name,
                NormalizedName = nameNormalized,
            };
        }

        #endregion

        #region RoleClaims

        public async Task AddRoleClaimAsync(RoleClaim roleClaim, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("AddRoleClaim"))
            {
                cmd.Parameters.AddWithValue("@roleId", roleClaim.RoleId);
                cmd.Parameters.AddWithValue("@claimType", roleClaim.ClaimType, null);
                cmd.Parameters.AddWithValue("@claimValue", roleClaim.ClaimValue, null);
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async Task<IList<RoleClaim>> GetRoleClaimsAsync(int roleId, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetRoleClaims"))
            {
                cmd.Parameters.AddWithValue("@roleId", roleId);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.ReadAll(ReadRoleClaim);
                }
            }
        }

        public async Task DeleteRoleClaimAsync(int roleId, string type, string value, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("DeleteRoleClaim"))
            {
                cmd.Parameters.AddWithValue("@roleId", roleId);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@value", value);

                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        private RoleClaim ReadRoleClaim(IDataReader rdr)
        {
            RoleClaim roleClaim = new RoleClaim
            {
                Id = rdr.GetValue<int>("Id"),
                RoleId = rdr.GetValue<int>("RoleId"),
                ClaimType = rdr.GetValue<string>("ClaimType"),
                ClaimValue = rdr.GetValue<string>("ClaimValue"),
            };
            return roleClaim;
        }

        #endregion

        #region UserClaims

        public async Task AddUserClaimAsync(UserClaim userClaim, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("AddUserClaim"))
            {
                cmd.Parameters.AddWithValue("@userId", userClaim.UserId);
                cmd.Parameters.AddWithValue("@claimType", userClaim.ClaimType, null);
                cmd.Parameters.AddWithValue("@claimValue", userClaim.ClaimValue, null);
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async Task<IList<UserClaim>> GetUserClaimsAsync(int userId, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetUserClaims"))
            {
                cmd.Parameters.AddWithValue("@userId", userId);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.ReadAll(ReadUserClaim);
                }
            }
        }

        public async Task ReplaceUserClaimAsync(UserClaim userClaim, UserClaim newUserClaim, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("ReplaceUserClaim"))
            {
                cmd.Parameters.AddWithValue("@userId", userClaim.UserId);
                cmd.Parameters.AddWithValue("@claimType", userClaim.ClaimType, null);
                cmd.Parameters.AddWithValue("@claimValue", userClaim.ClaimValue, null);
                cmd.Parameters.AddWithValue("@newClaimType", newUserClaim.ClaimType, null);
                cmd.Parameters.AddWithValue("@newClaimValue", newUserClaim.ClaimValue, null);
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async Task DeleteUserClaimAsync(int userId, string type, string value, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("DeleteUserClaim"))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@claimType", type);
                cmd.Parameters.AddWithValue("@claimValue", value);
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async Task<IList<User>> GetUsersForClaim(string type, string value, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetUsersForClaim"))
            {
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@value", value);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.ReadAll(ReadUser);
                }
            }
        }

        private UserClaim ReadUserClaim(IDataReader rdr)
        {
            UserClaim userClaim = new UserClaim
            {
                Id = rdr.GetValue<int>("Id"),
                UserId = rdr.GetValue<int>("UserId"),
                ClaimType = rdr.GetValue<string>("ClaimType"),
                ClaimValue = rdr.GetValue<string>("ClaimValue"),
            };
            return userClaim;
        }

        #endregion

        #region Logging

        public async Task LogError(Exception error, CancellationToken token)
        {
            if (error == null)
                return;
            if (error is OperationCanceledException)
                return;

            await LogError(error.ToString(true), token);
        }

        public async Task LogWarning(Exception error, CancellationToken token)
        {
            if (error == null)
                return;
            if (error is OperationCanceledException)
                return;

            await LogWarning(error.ToString(true), token);
        }

        public async Task LogError(string message, CancellationToken token)
        {
            await TryAddLog('e', message, token);
        }

        public async Task LogWarning(string message, CancellationToken token)
        {
            await TryAddLog('w', message, token);
        }

        public async Task LogInfo(string message, CancellationToken token)
        {
            await TryAddLog('i', message, token);
        }

        public async Task TryAddLog(char type, string message, CancellationToken token)
        {
            try
            {
                await AddLogAsync(type, message, token);
            }
            catch (Exception)
            {
            }
        }

        public async Task AddLogAsync(char type, string message, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("AddLog"))
            {
                cmd.Parameters.AddWithValue("@type", type.ToString());
                cmd.Parameters.AddWithValue("@message", message);
                await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async Task<int> DeleteOldLogsAsync(TimeSpan? maxAge, CancellationToken token)
        {
            DateTime? minTime = null;

            if (maxAge.HasValue)
                minTime = DateTime.UtcNow - maxAge;

            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("DeleteOldLogs"))
            {
                cmd.Parameters.AddWithValue("@minTime", minTime, null);

                return await cmd.ExecuteNonQueryAsync(token);
            }
        }

        public async Task<IList<LogItem>> GetUnreadLogs(string type, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetUnreadLogs"))
            {
                cmd.Parameters.AddWithValue("@type", type, null);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    return rdr.ReadAll(ReadLog);
                }
            }
        }

        private LogItem ReadLog(IDataReader rdr)
        {
            LogItem item = new LogItem()
            {
                Id = rdr.GetValue<int>("Id"),
                Timestamp = rdr.GetValue<DateTime>("Timestamp"),
                Type = rdr.GetValue<string>("Type"),
                IsRead = rdr.GetValue<bool>("IsRead"),
                Message = rdr.GetValue<string>("Message")
            };

            return item;
        }

        #endregion

        #region Admin

        public async Task<SiteInfo> GetAdminStats(DateTime maxAge, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetAdminStats"))
            {
                cmd.Parameters.AddWithValue("@maxAge", maxAge);
                cmd.Parameters.AddWithValue("@logItems", 50);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    SiteInfo info = new SiteInfo();

                    if (await rdr.ReadAsync(token))
                    {
                        info.UsersCount = rdr.GetValue<int>("UsersCount");
                        info.LoginAttemptCount = rdr.GetValue<int>("LoginAttemptCount");

                        info.UsersNewCount = rdr.GetValue<int>("UsersNewCount");
                    }

                    await rdr.NextResultAsync(token);

                    while (await rdr.ReadAsync(token))
                    {
                        LogItem item = ReadLog(rdr);
                        info.Logs.Add(item);
                    }

                    return info;
                }
            }
        }

        public async Task<List<UserInfo>> GetUsersInfo(DateTime maxAge, CancellationToken token)
        {
            using (SqlConnection con = await GetConnectionAsync(token))
            using (SqlCommand cmd = con.StoredProcedure("GetUserAllInfo"))
            {
                cmd.Parameters.AddWithValue("@maxAge", maxAge);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync(token))
                {
                    List<UserInfo> infos = new List<UserInfo>();
                    while (await rdr.ReadAsync(token))
                    {
                        UserInfo info = new UserInfo()
                        {
                            User = ReadUser(rdr),
                            LoginCount = rdr.GetValue<int>("LoginCount"),
                        };

                        infos.Add(info);
                    }
                    return infos;
                }

            }
        }

        #endregion
    }
}



