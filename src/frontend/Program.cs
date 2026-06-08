using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Frontend.Data;
using Frontend.Models.Entities;
using Frontend.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=db;Database=dec_db;User=dec_user;Password=dec_password;";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Centralized Identity Configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = "DEC.Auth";
});

builder.Services.AddHttpContextAccessor();

// HttpClient for the Django Financial Engine
builder.Services.AddHttpClient("FinancialEngine", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["FinancialEngine:BaseUrl"] ?? "http://calculation-engine:8000/api/");
    client.DefaultRequestVersion = HttpVersion.Version11;
    client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Ensure we don't use chunked encoding which Django's runserver struggles with
    UseProxy = false,
    AllowAutoRedirect = false
});

// Domain Services
// builder.Services.AddScoped<IEducationService, EducationService>(); // Will be refactored to use DbContext
builder.Services.AddScoped<ISimulationService, SimulationService>(); 

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
