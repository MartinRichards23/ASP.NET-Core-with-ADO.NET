using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WebsiteTemplate.Data;
using WebsiteTemplate.Models;

namespace WebsiteTemplate
{
    /// <summary>
    /// Implementation of custom role store
    /// </summary>
    public class MyRoleStore : IRoleStore<Role>, IRoleClaimStore<Role>
    {
        readonly Database database;
        readonly ILogger logger;
        bool disposed;

        public MyRoleStore(Database database, ILogger<MyRoleStore> logger)
        {
            this.database = database;
            this.logger = logger;
        }

        #region IRoleStore

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            try
            {
                await database.AddRoleAsync(role, cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "CreateAsync");
                return IdentityResult.Failed(new IdentityError() { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            try
            {
                await database.UpdateRoleAsync(role, cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UpdateAsync");
                return IdentityResult.Failed(new IdentityError() { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            try
            {
                await database.DeleteRoleAsync(role.Id, cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DeleteAsync");
                return IdentityResult.Failed(new IdentityError() { Description = ex.Message });
            }
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return await database.GetRoleAsync(int.Parse(roleId), cancellationToken);
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return await database.GetRoleByNameAsync(normalizedRoleName, cancellationToken);
        }

        public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return await Task.FromResult(role.NormalizedName);
        }

        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return await Task.FromResult(role.Id.ToString());
        }

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return await Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            role.Name = roleName;
            return Task.CompletedTask;
        }

        #endregion

        #region IRoleClaimStore

        public async Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();

            IList<Claim> claims = new List<Claim>();
            IList<RoleClaim> userClaims = await database.GetRoleClaimsAsync(role.Id, cancellationToken);
            foreach (RoleClaim rc in userClaims)
            {
                claims.Add(new Claim(rc.ClaimType, rc.ClaimValue));
            }
            return claims;
        }

        public async Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();

            await database.AddRoleClaimAsync(new RoleClaim() { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value, }, cancellationToken);
        }

        public async Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();

            await database.DeleteRoleClaimAsync(role.Id, claim.Type, claim.Value, cancellationToken);
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
