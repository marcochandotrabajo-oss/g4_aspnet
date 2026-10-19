using G4AspNet.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace G4AspNet.Controllers;

/// <summary>Página de inicio del proyecto, sin entidades del dominio todavía.</summary>
public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
    });
}
