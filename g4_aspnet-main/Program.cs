using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using G4AspNet.Data;
using G4AspNet.Models;
using G4AspNet.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se ha configurado la conexión DefaultConnection.");

// DbContext es scoped: una petición HTTP usa la misma unidad de trabajo de EF Core.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Identity gestiona usuarios, cookies de sesión, contraseñas y roles.
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.AccessDeniedPath = "/account/accessdenied";
    options.Cookie.Name = "G4AspNet.Auth";
    options.SlidingExpiration = true;
    options.Events.OnValidatePrincipal = async context =>
    {
        if (context.Principal is null)
        {
            context.RejectPrincipal();
            return;
        }

        var userManager = context.HttpContext.RequestServices
            .GetRequiredService<UserManager<ApplicationUser>>();
        var user = await userManager.GetUserAsync(context.Principal);
        if (user is null || !user.IsActive)
        {
            context.RejectPrincipal();
        }
    };
});

builder.Services.AddControllersWithViews(options =>
{
    // Protege automáticamente los formularios POST contra peticiones falsificadas.
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.Configure<ImageStorageOptions>(
    builder.Configuration.GetSection("FileUploads:Images"));
var maxImageSize = builder.Configuration.GetValue(
    "FileUploads:Images:MaxFileSizeBytes",
    5 * 1024 * 1024);
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = maxImageSize + (512 * 1024);
});
builder.Services.AddSingleton<ImageStorage>();
builder.Services.AddScoped<UserService>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var appDataDirectory = Path.Combine(app.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(appDataDirectory);

using (var scope = app.Services.CreateScope())
{
    await DbInitializer.InitializeAsync(scope.ServiceProvider);
}

app.Run();
