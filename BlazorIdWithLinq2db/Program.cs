using BlazorIdWithLinq2db;
using BlazorIdWithLinq2db.Components;
using BlazorIdWithLinq2db.Components.Account;
using BlazorIdWithLinq2dbModels;
using Linq2DB.CustomIdentity.Data;
using Linq2DB.CustomIdentity.DefaultConnectionFactory;
using LinqToDB.Data;
using Linq2DB.CustomIdentity.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using BlazorIdWithLinq2db.Data;
using LinqToDB;


var builder = WebApplication.CreateBuilder(args);
// Set up Linq2DB connection
DataConnection.DefaultSettings = new Linq2dbSettings(builder);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<RoleManager<AspNetRole>>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;

    })
    .AddIdentityCookies();



builder.Services.AddIdentityCore<AspNetUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<AspNetRole>()
    //.AddRoleManager<RoleManager<IdentityRole>>()
    .AddUserStore<UserStore<string, AspNetUser, AspNetRole, AspNetUserRole>>()
    .AddLinqToDBStores(new DefaultConnectionFactory()) // add DI services for custom identity
    
    .AddSignInManager()

    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<AspNetUser>, IdentityNoOpEmailSender>();




var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    //await CreateTables();
    await SeedRoles(app.Services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    //app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorIdWithLinq2db.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
async Task SeedRoles(IServiceProvider services)
{
    using var scope = services.CreateScope();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AspNetRole>>();
    string[] roles = new string[] { "Admin", "Member", "Moderators" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new AspNetRole(role));

        }
    }
}
async Task CreateTables() {
    using (var db = new ApplicationDataConnection())
    {
        TryCreateTable<AspNetUser>(db);
        TryCreateTable<AspNetRole>(db);
        //TryCreateTable<LinqToDB.Identity.IdentityUserClaim<string>>(db);
        //TryCreateTable<LinqToDB.Identity.IdentityRoleClaim<string>>(db);
        //TryCreateTable<LinqToDB.Identity.IdentityUserLogin<string>>(db);
        TryCreateTable<AspNetUserRole>(db);
        //TryCreateTable<LinqToDB.Identity.IdentityUserToken<string>>(db);
    }
}
void TryCreateTable<T>(ApplicationDataConnection db)
            where T : class
{
    try
    {
        db.CreateTable<T>();
    }
    catch
    {
        //
    }
}