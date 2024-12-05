using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

// Configuración de sesiones
builder.Services.AddAuthorization(options => {
    options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireClaim(ClaimTypes.Role).Build();
    options.AddPolicy("ElevatedRights", policy => policy.RequireRole("Administrador", "Operador", "Radicador", "Doctor"));
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => {
    options.ExpireTimeSpan = TimeSpan.FromHours(12);
    //options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
    options.SlidingExpiration = true;
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.LoginPath = "/Auth/Index";
    options.LogoutPath = "/Auth/LogOut";
});
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromHours(12); 
    //options.IdleTimeout = TimeSpan.FromMinutes(120); 
});
builder.Services.AddMvc();

// Register the BearerTokenHandler
builder.Services.AddTransient<BearerTokenHandler>();

// Configure HttpClient with the BearerTokenHandler
builder.Services.AddHttpClient("HttpApiClient")
        .AddHttpMessageHandler<BearerTokenHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

app.Run();
