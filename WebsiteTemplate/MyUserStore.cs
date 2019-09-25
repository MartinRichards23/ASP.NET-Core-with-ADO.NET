using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using SystemPlus.Collections.Generic;
using WebsiteTemplate.Data;
using WebsiteTemplate.Models;

namespace WebsiteTemplate
{
    /// <summary>
    /// Implementation of custom user store
    /// </summary>
    public class MyUserStore : IUserStore<User>, IUserPasswordStore<User>, IUserEmailStore<User>, IUserSecurityStampStore<User>, IUserPhoneNumberStore<User>, IUserLoginStore<User>,
        IUserRoleStore<User>, IUserClaimStore<User>,
        IUserTwoFactorStore<User>, IUserAuthenticatorKeyStore<User>, IUserTwoFactorRecoveryCodeStore<User>,
        IUserLockoutStore<User>
    {
        readonly Database database;
        readonly ILogger logger;
        bool disposed;

        public MyUserStore(Database database, ILogger<MyUserStore> logger)
        {
            this.database = database;
            this.logger = logger;
        }

        #region IUserstore

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            try
            {
                await database.AddUserAsync(user, cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Create");
                return IdentityResult.Failed(new IdentityError() { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            try
            {
                await database.UpdateUserAsync(user, cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Update");
                return IdentityResult.Failed(new IdentityError() { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            try
            {
                await database.DeleteUserAsync(user.Id, cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DeleteAsync");
                return IdentityResult.Failed(new IdentityError() { Description = ex.Message });
            }
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            try
            {
                int id = int.Parse(userId);
                return await database.GetUserAsync(id, cancellationToken);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            return await database.GetUserByUserNameAsync(normalizedUserName, cancellationToken);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            return Task.FromResult(user.Email);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.FromResult(user.PasswordHash != null);
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.FromResult(user.PasswordHash);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserEmailStore

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return await database.GetUserByEmailAsync(normalizedEmail, cancellationToken);
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserPhoneNumberStore

        public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.PhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserSecurityStampStore

        public Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        public Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.FromResult(user.SecurityStamp);
        }

        #endregion

        #region IUserLoginStore

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            UserLogin userLogin = new UserLogin()
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName,
                ProviderKey = login.ProviderKey,
            };

            await database.AddUserLoginAsync(userLogin, cancellationToken);
        }

        public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            await database.DeleteUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            IList<UserLoginInfo> infos = new List<UserLoginInfo>();
            IList<UserLogin> logins = await database.GetUserLoginsAsync(user.Id, cancellationToken);

            foreach (UserLogin login in logins)
            {
                infos.Add(new UserLoginInfo(login.LoginProvider, login.ProviderKey, login.ProviderDisplayName));
            }

            return infos;
        }

        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            UserLogin login = await database.GetUserLoginAsync(loginProvider, providerKey, cancellationToken);
            if (login == null)
                return null;

            return await database.GetUserAsync(login.UserId, cancellationToken);
        }

        #endregion

        #region IUserTwoFactorStore

        public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.FromResult(user.TwoFactorEnabled);
        }

        #endregion

        #region IUserAuthenticatorKeyStore

        private const string AuthenticatorStoreLoginProvider = "[AspNetAuthenticatorStore]";
        private const string AuthenticatorKeyTokenName = "AuthenticatorKey";
        private const string RecoveryCodeTokenName = "RecoveryCodes";

        public async Task SetAuthenticatorKeyAsync(User user, string key, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            await database.SetUserTokenAsync(user, AuthenticatorStoreLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);
        }

        public async Task<string> GetAuthenticatorKeyAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return await database.GetUserTokenAsync(user, AuthenticatorStoreLoginProvider, AuthenticatorKeyTokenName, cancellationToken);
        }

        #endregion

        #region IUserTwoFactorRecoveryCodeStore

        public async Task ReplaceCodesAsync(User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            string mergedCodes = string.Join(";", recoveryCodes);
            await database.SetUserTokenAsync(user, AuthenticatorStoreLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
        }

        public async Task<bool> RedeemCodeAsync(User user, string code, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            string mergedCodes = await database.GetUserTokenAsync(user, AuthenticatorStoreLoginProvider, RecoveryCodeTokenName, cancellationToken);
            mergedCodes = mergedCodes ?? "";

            string[] splitCodes = mergedCodes.Split(';');
            if (splitCodes.Contains(code))
            {
                List<string> updatedCodes = new List<string>(splitCodes.Where(s => s != code));
                await ReplaceCodesAsync(user, updatedCodes, cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<int> CountCodesAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            string mergedCodes = await database.GetUserTokenAsync(user, AuthenticatorStoreLoginProvider, RecoveryCodeTokenName, cancellationToken);
            mergedCodes = mergedCodes ?? "";

            if (mergedCodes.Length > 0)
            {
                return mergedCodes.Split(';').Length;
            }

            return 0;
        }

        #endregion

        #region IUserLockoutStore

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.FromResult(user.LockoutEnd);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.LockoutEnd = lockoutEnd;
            return Task.CompletedTask;
        }

        public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserRoleStore

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            await database.AddToRoleAsync(user, roleName, cancellationToken);
        }

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            await database.RemoveFromRoleAsync(user, roleName, cancellationToken);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var res = await database.GetRolesAsync(user, cancellationToken);
            return res.Select(r => r.Name).ToIList();
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return await database.IsInRoleAsync(user, roleName, cancellationToken);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return await database.GetUsersInRoleAsync(roleName, cancellationToken);
        }

        #endregion

        #region IUserClaimStore

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            IList<Claim> claims = new List<Claim>();
            IList<UserClaim> userClaims = await database.GetUserClaimsAsync(user.Id, cancellationToken);
            foreach (UserClaim uc in userClaims)
            {
                claims.Add(new Claim(uc.ClaimType, uc.ClaimValue));
            }

            return claims;
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            foreach (Claim c in claims)
            {
                await database.AddUserClaimAsync(new UserClaim() { UserId = user.Id, ClaimType = c.Type, ClaimValue = c.Value }, cancellationToken);
            }
        }

        public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            UserClaim userClaim = new UserClaim() { UserId = user.Id, ClaimType = claim.Type, ClaimValue = claim.Value };
            UserClaim newUserClaim = new UserClaim() { UserId = user.Id, ClaimType = newClaim.Type, ClaimValue = newClaim.Value };
            await database.ReplaceUserClaimAsync(userClaim, newUserClaim, cancellationToken);
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            foreach (Claim c in claims)
            {
                await database.DeleteUserClaimAsync(user.Id, c.Type, c.Value, cancellationToken);
            }
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return await database.GetUsersForClaim(claim.Type, claim.Value, cancellationToken);
        }

        #endregion

        protected void ThrowIfDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// Dispose the store
        /// </summary>
        public void Dispose()
        {
            disposed = true;
        }
    }
}
