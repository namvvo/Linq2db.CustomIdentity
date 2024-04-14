using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Linq2DB.CustomIdentity.Data;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace Linq2DB.CustomIdentity.DependencyInjection
{
    public static class IdentityLinq2DbBuilderExtensions
    {
        public static IdentityBuilder AddLinqToDBStores(this IdentityBuilder builder, IConnectionFactory factory)
        {
            return AddLinqToDBStores(builder, factory,
                typeof(string),
                //typeof(LinqToDB.Identity.IdentityUserClaim<string>),
                typeof(IdentityUserRole<string>)
                //typeof(LinqToDB.Identity.IdentityUserLogin<string>),
                //typeof(LinqToDB.Identity.IdentityUserToken<string>),
                //typeof(LinqToDB.Identity.IdentityRoleClaim<string>)
                );
        }
        public static IdentityBuilder AddLinqToDBStores(this IdentityBuilder builder,
                                                        IConnectionFactory factory,
                                                        Type keyType,
                                                        //Type userClaimType,
                                                        Type userRoleType
                                                        //Type userLoginType,
                                                        //Type userTokenType,
                                                        //Type roleClaimType
            )
        {
            builder.Services.AddSingleton(factory);

            builder.Services.TryAdd(GetDefaultServices(
                keyType,
                builder.UserType,
                //userClaimType,

                //userLoginType,
                //userTokenType,
                builder.RoleType,
                 userRoleType
                //roleClaimType
                ));

            return builder;
        }
        private static IServiceCollection GetDefaultServices(Type keyType,
                                                             Type userType,
                                                             //Type userClaimType,

                                                             //Type userLoginType,
                                                             //Type userTokenType,
                                                             Type roleType,
                                                             Type userRoleType
                                                             //Type roleClaimType
            )
        {
            //UserStore<TKey,TUser, TRole, TUserRole>
            var userStoreType = typeof(UserStore<,,,>).MakeGenericType(keyType, userType, roleType, userRoleType);
            // RoleStore<TRole, TRoleClaim>
            var roleStoreType = typeof(RoleStore<>).MakeGenericType(roleType);

            var services = new ServiceCollection();
            services.AddScoped(
                typeof(IUserStore<>).MakeGenericType(userType),
                userStoreType);
            services.AddScoped(
                typeof(IRoleStore<>).MakeGenericType(roleType),
                roleStoreType);
            return services;
        }
    }
}
