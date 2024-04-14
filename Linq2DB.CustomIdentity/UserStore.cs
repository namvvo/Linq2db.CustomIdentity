
using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Data;

namespace Linq2DB.CustomIdentity.Data
{
    public class UserStore<TKey,TUser, TRole, TUserRole> :
        IUserStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserEmailStore<TUser>,
        IUserRoleStore<TUser>
        where TUser : IdentityUser<TKey>, new()
        where TRole : IdentityRole<TKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TKey : IEquatable<TKey>
    {

        private readonly IConnectionFactory _factory;
        public UserStore(IConnectionFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _factory = factory;
            
        }

        protected DataConnection GetConnection() => _factory.GetConnection();
        #region IUserStore
        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            try
            {
                using var db = GetConnection();
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
                using var db = GetConnection();
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
            using var db = GetConnection();
            var id = ConvertIdFromString(userId);
            var user = db.GetTable<TUser>().FirstOrDefaultAsync(_ => _.Id.Equals(id), cancellationToken);

            return Task.FromResult(user as TUser);
        }

        public async Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using var db = GetConnection();
            var user = await db.GetTable<TUser>().FirstOrDefaultAsync(_ => _.NormalizedUserName.ToUpper() == normalizedUserName);

            return await Task.FromResult(user as TUser);
        }

        public Task<string?> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual TKey ConvertIdFromString(string id)
        {
            if (id == null)
                return default(TKey);
            return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
        }
        public virtual string ConvertIdToString(TKey id)
        {
            if (Equals(id, default(TKey)))
                return null;
            return id.ToString();
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
       
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(ConvertIdToString(user.Id));
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
                using var db = GetConnection();
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
            using var db = GetConnection();
            var user = await db.GetTable<TUser>().FirstOrDefaultAsync(_ => _.Email.ToUpper() == normalizedEmail);
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

        public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            using var db = GetConnection();
            var role = await db.GetTable<TRole>().FirstOrDefaultAsync(_ => _.Name == roleName);
            if (role is null) throw new ArgumentNullException($"{roleName} does not exist");
            var newUserRole = new TUserRole()
            {
                UserId = user.Id,
                RoleId = role.Id
            };
            db.Insert(newUserRole);
        }

        public async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            using var db = GetConnection();
            var role = await db.GetTable<TRole>().FirstOrDefaultAsync(_ => _.Name == roleName);
            if (role is null) throw new ArgumentNullException($"{roleName} does not exist");
            var userRole = db.GetTable<TUserRole>().FirstOrDefaultAsync(_ => _.UserId.Equals(user.Id)
                         && _.RoleId.Equals(role.Id));
            await db.DeleteAsync(userRole);
        }
        private readonly RoleManager<IdentityRole> _roleManager;


        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            using var db = GetConnection();
            var userRoles = db.GetTable<TUserRole>().Where(_ => _.UserId.Equals(user.Id));
            IList<string> roles = new List<string>();
            foreach (var item in userRoles)
            {
                var role = await db.GetTable<TRole>().FirstOrDefaultAsync(_ => _.Id.Equals(item.RoleId));
                if (role != null) roles.Add(role.Name);
            }
            return await Task.FromResult(roles);
        }

        public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            using var db = GetConnection();
            var role = await db.GetTable<TRole>().FirstOrDefaultAsync(_ => _.Name == roleName);
            if (role is null) throw new ArgumentNullException($"{roleName} does not exist");
            var userRole = await db.GetTable<TUserRole>()
                                   .FirstOrDefaultAsync(f =>
                                                        f.UserId.Equals(user.Id)
                                                        && f.RoleId.Equals(role.Id));

            return userRole != null;
        }

        public Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
