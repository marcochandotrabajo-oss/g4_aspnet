using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using G4AspNet.Models;
using G4AspNet.ViewModels.Account;

namespace G4AspNet.Controllers;

/// <summary>Registro, login y logout mediante ASP.NET Core Identity.</summary>
public sealed class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl, bool loggedOut = false) => View(new LoginViewModel
    {
        ReturnUrl = returnUrl,
        ShowLoggedOutMessage = loggedOut
    });

    [HttpPost]
    [AllowAnonymous]
    /// <summary>Identity necesita async para validar la contraseña y crear la cookie.</summary>
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var identifier = model.Username.Trim();
        var user = await userManager.FindByNameAsync(identifier)
            ?? await userManager.FindByEmailAsync(identifier);
        if (user is null || !user.IsActive)
        {
            ModelState.AddModelError(string.Empty, "El usuario o la contraseña no son válidos.");
            return View(model);
        }

        var result = await signInManager.PasswordSignInAsync(
            user,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true);
        if (result.Succeeded)
        {
            return RedirectToLocal(model.ReturnUrl);
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "La cuenta se ha bloqueado temporalmente por demasiados intentos.");
            return View(model);
        }

        ModelState.AddModelError(string.Empty, "El usuario o la contraseña no son válidos.");
        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Username.Trim(),
            Email = model.Email.Trim(),
            DisplayName = string.IsNullOrWhiteSpace(model.DisplayName)
                ? model.Username.Trim()
                : model.DisplayName.Trim(),
            IsActive = true,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            AddIdentityErrors(result);
            return View(model);
        }

        var roleResult = await userManager.AddToRoleAsync(user, RoleNames.User);
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            AddIdentityErrors(roleResult);
            return View(model);
        }

        await signInManager.SignInAsync(user, isPersistent: false);
        TempData["Message"] = "Tu cuenta se ha creado correctamente.";
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login), new { loggedOut = true });
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied() => View();

    /// <summary>Evita que returnUrl pueda redirigir a una web externa.</summary>
    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl!);
        }

        return RedirectToAction("Index", "Home")!;
    }

    private void AddIdentityErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
