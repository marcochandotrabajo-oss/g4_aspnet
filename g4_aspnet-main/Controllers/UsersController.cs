using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using G4AspNet.Models;
using G4AspNet.Services;
using G4AspNet.Utilities;
using G4AspNet.ViewModels.Users;

namespace G4AspNet.Controllers;

[Authorize(Roles = RoleNames.Admin)]
/// <summary>Panel administrativo de cuentas, roles, estado y avatares.</summary>
public sealed class UsersController : Controller
{
    private readonly UserService userService;

    public UsersController(UserService userService)
    {
        this.userService = userService;
    }

    [HttpGet]
    /// <summary>Identity ofrece los roles de forma asíncrona.</summary>
    public async Task<IActionResult> Index(string? search)
    {
        return View(new UserIndexViewModel
        {
            Search = search,
            Users = await userService.SearchAsync(search)
        });
    }

    [HttpGet]
    public IActionResult Create() => View(new CreateUserViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await userService.CreateAsync(model);
        if (!result.Succeeded)
        {
            AddIdentityErrors(result);
            return View(model);
        }

        TempData["Message"] = "Usuario creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Details(string id)
    {
        var model = userService.GetProfile(id);
        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var model = await userService.GetEditModelAsync(id);
        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, EditUserViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await userService.UpdateAsync(model, User.GetRequiredUserId());
        if (!result.Succeeded)
        {
            AddIdentityErrors(result);
            return View(model);
        }

        TempData["Message"] = "Usuario actualizado correctamente.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    public IActionResult Delete(string id)
    {
        var model = userService.GetProfile(id);
        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var result = await userService.DeleteAsync(id, User.GetRequiredUserId());
        if (!result.Succeeded)
        {
            TempData["Error"] = string.Join(" ", result.Errors.Select(error => error.Description));
            return RedirectToAction(nameof(Details), new { id });
        }

        TempData["Message"] = "Usuario eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private void AddIdentityErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
