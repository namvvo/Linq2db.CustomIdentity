using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Identity;

namespace Linq2DB.CustomIdentity.Data
{
    public class RoleStore<TRole> : IRoleStore<TRole>
        where TRole : IdentityRole<string>
    {
        private readonly IConnectionFactory _factory;
        public RoleStore(IConnectionFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _factory = factory;

        }
        protected DataConnection GetConnection() => _factory.GetConnection();

        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            if (role is null) throw new ArgumentNullException(nameof(role));
            try
            {
                using var db = GetConnection();
                await db.InsertAsync(role);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            if (role is null) throw new ArgumentNullException(nameof(role));
            try
            {
                using var db = GetConnection();
                await db.UpdateAsync(role);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            if (role is null) throw new ArgumentNullException(nameof(role));
            try
            {
                using var db = GetConnection();
                await db.DeleteAsync(role);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            return await Task.FromResult(role.Id);
        }

        public async Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return await Task.FromResult(role.Name);
        }

        public async Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            await Task.CompletedTask;
        }

        public async Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return await Task.FromResult(role.NormalizedName);
        }

        public async Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            await Task.CompletedTask;
        }

        public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            using var db = GetConnection();
            return await db.GetTable<TRole>().FirstOrDefaultAsync(_ => _.Id.Equals(roleId), cancellationToken);
        }

        public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            using var db = GetConnection();
            return await db.GetTable<TRole>()
                .FirstOrDefaultAsync(r => r.NormalizedName.Equals(normalizedRoleName), cancellationToken);
        }

        public void Dispose()
        {
            // Dispose any resources if needed
        }
    }
}
