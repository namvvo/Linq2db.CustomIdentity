using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Linq2DB.CustomIdentity.Data
{
    public class CustomRoleManager<TRole> : RoleManager<TRole>
    where TRole : IdentityRole
    {
        public CustomRoleManager(IRoleStore<TRole> store,
                             IEnumerable<IRoleValidator<TRole>> roleValidators,
                             ILookupNormalizer keyNormalizer,
                             IdentityErrorDescriber errors,
                             ILogger<RoleManager<TRole>> logger)
        : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
        //public override async Task<IdentityResult> AddToRoleAsync(TRole role, string userId)
        //{
        //    // Custom logic before adding user to role
        //    // For example, you can perform additional checks or validations here

        //    return await base.AddToRoleAsync(role, userId);
        //}
    }
}
