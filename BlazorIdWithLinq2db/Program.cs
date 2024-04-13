using BlazorIdWithLinq2db;
using BlazorIdWithLinq2db.Components;
using BlazorIdWithLinq2db.Components.Account;
//using BlazorIdWithLinq2db.Data;
using BlazorIdWithLinq2dbModels;
using Linq2DB.CustomIdentity;
using Linq2DB.CustomIdentity.Data;
using Linq2DB.CustomIdentity.DefaultConnectionFactory;
using LinqToDB.Data;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;


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
builder.Services.AddScoped<IRoleStore<AspNetRole>, CustomRoleStore<AspNetRole>>();
builder.Services.AddScoped<RoleManager<AspNetRole>>();

builder.Services.AddScoped<UserStore<string, AspNetUser, AspNetRole, AspNetUserRole>>();

builder.Services.AddSingleton<IConnectionFactory, DefaultConnectionFactory>();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;

    })
    .AddIdentityCookies();



builder.Services.AddIdentityCore<AspNetUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<AspNetRole>()
    .AddRoleManager<RoleManager<AspNetRole>>()
    .AddUserStore<UserStore<string, AspNetUser, AspNetRole, AspNetUserRole>>()
    .AddSignInManager()

    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<AspNetUser>, IdentityNoOpEmailSender>();




var app = builder.Build();

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
