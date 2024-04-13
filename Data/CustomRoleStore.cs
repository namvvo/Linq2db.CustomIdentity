using Microsoft.AspNetCore.Identity;

namespace BlazorIdWithLinq2db.Data
{
    public class CustomRoleStore<TRole> : IRoleStore<TRole>
        where TRole: IdentityRole
    {
       

        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
         
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            //_dbContext.Attach(role).State = EntityState.Modified;
            //await _dbContext.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            //_dbContext.Set<TRole>().Remove(role);
            //await _dbContext.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
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
            return null;
            //return await _dbContext.Set<TRole>().FindAsync(new object[] { roleId }, cancellationToken);
        }

        public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return null;
            //return await _dbContext.Set<TRole>()
            //    .FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
        }

        public void Dispose()
        {
            // Dispose any resources if needed
        }
    }
}
