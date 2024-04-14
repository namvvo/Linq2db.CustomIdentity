A simple customized asp.net identity with Linq2Db data provider instead of Entity Framework. 

How to use:

On Nuget console: install-package namvvo.linq2db.customidentity

On the Program.cs:

builder.Services.AddIdentityCore<AspNetUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<AspNetRole>()
    .AddRoleManager<RoleManager<AspNetRole>>()
    .AddUserStore<UserStore<string, AspNetUser, AspNetRole, AspNetUserRole>>()
    .AddLinqToDBStores(new DefaultConnectionFactory()) // add DI services for custom identity   
    .AddSignInManager()
    .AddDefaultTokenProviders();

AddLinqToDbStores will automatically inject all the necessary services which includes Factory connection DI, UserStore and Role Store. I'll be adding along more later on.
