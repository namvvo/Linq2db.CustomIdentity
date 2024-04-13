using BlazorIdWithLinq2dbModels;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace BlazorIdWithLinq2db.Data
{
    public class CustomUserStore<TUser>(RoleManager<AspNetRole> roleManager):
        IUserStore<TUser>,
        IUserPasswordStore<TUser>,
         IUserEmailStore<TUser>,
         IUserRoleStore<TUser>
        where TUser : IdentityUser
    {
        #region IUserStore
        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            try
            {
                using var db = new TestDataDB();
                await db.InsertAsync(user);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            try
            {
                using var db = new TestDataDB();
                await db.DeleteAsync(user);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = ex.Message
                });
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<TUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!String.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException(nameof(userId));
            using var db = new TestDataDB();

            var user = db.AspNetUsers.Find(userId);

            return Task.FromResult(user as TUser);
        }

        public async Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using var db = new TestDataDB();
            var user = await db.AspNetUsers.FirstOrDefaultAsync(f => f.NormalizedUserName.ToUpper() == normalizedUserName);

            return await Task.FromResult(user as TUser);
        }

        public Task<string?> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }



        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string?> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }


        public Task SetNormalizedUserNameAsync(TUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }



        public Task SetUserNameAsync(TUser user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user is null) throw new ArgumentNullException(nameof(user));
            try
            {
                using var db = new TestDataDB();
                await db.UpdateAsync(user);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }
        #endregion
        #region IUserPasswordStore
        public Task SetPasswordHashAsync(TUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }
        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            // Check if the user has a password set
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }
        public Task<string?> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            // Implement logic to retrieve the password hash from your data store
            return Task.FromResult(user.PasswordHash);
        }


        #endregion 
        #region  IUserEmailStore
        public Task SetEmailAsync(TUser user, string? email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string?> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public async Task<TUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            using var db = new TestDataDB();
            var user = await db.AspNetUsers.FirstOrDefaultAsync(f => f.Email.ToUpper() == normalizedEmail);
            return await Task.FromResult(user as TUser);
        }

        public Task<string?> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(TUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedEmail;
            return Task.CompletedTask;
        }

        #endregion
        #region IUserRoleStore
        //private readonly UserManager<TUser> _userManager;


        public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
           
            using var db = new TestDataDB();
            
            var role = await db.AspNetRoles.FirstOrDefaultAsync(f => f.Name == roleName);
            if (role is null) throw new ArgumentNullException($"{roleName} does not exist");
            var newUserRole = new AspNetUserRole()
            {
                UserId = user.Id,
                RoleId = role.Id
            };
            db.Insert(newUserRole);
        }

        public async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            using var db = new TestDataDB();
            var role = await db.AspNetRoles.FirstOrDefaultAsync(f => f.Name == roleName);
            if (role is null) throw new ArgumentNullException($"{roleName} does not exist");
            var userRole = db.AspNetUserRoles.FirstOrDefaultAsync(f => f.UserId == user.Id && f.RoleId == role.Id);
            await db.DeleteAsync(userRole);
        }

        public Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            using var db = new TestDataDB();
            var userRoles = db.AspNetUserRoles.Where(f => f.UserId == user.Id).LoadWith(l => l.Role);
            IList<string> roles = new List<string>();
            foreach (var item in userRoles)
                roles.Add(item.Role.Name);
            return Task.FromResult(roles);
        }

        public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            using var db = new TestDataDB();
            var role = await db.AspNetRoles.FirstOrDefaultAsync(f => f.Name == roleName);
            if (role is null) throw new ArgumentNullException($"{roleName} does not exist");
            var userRole = await db.AspNetUserRoles.FirstOrDefaultAsync(f =>
            f.UserId == user.Id
            && f.RoleId == role.Id);

            return userRole != null;
        }

        public Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
