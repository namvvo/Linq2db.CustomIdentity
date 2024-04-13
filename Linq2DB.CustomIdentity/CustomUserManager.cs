
//using LinqToDB;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;

//namespace Linq2DB.CustomIdentity.Data
//{
//    public class CustomUserManager<TUser> : UserManager<TUser>
//     where TUser : IdentityUser
//    {
//        public CustomUserManager(IUserStore<TUser> store,
//                             IOptions<IdentityOptions> optionsAccessor,
//                             IPasswordHasher<TUser> passwordHasher,
//                             IEnumerable<IUserValidator<TUser>> userValidators,
//                             IEnumerable<IPasswordValidator<TUser>> passwordValidators,
//                             ILookupNormalizer keyNormalizer,
//                             IdentityErrorDescriber errors,
//                             IServiceProvider services,
//                             ILogger<UserManager<TUser>> logger,
//                             CustomUserStore<TUser> customUserStore)
//        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
//        {
//        }


//    }
//}
