using System.ComponentModel.DataAnnotations;

namespace G4AspNet.ViewModels.Account;

/// <summary>Campos del formulario de inicio de sesión.</summary>
public sealed class LoginViewModel
{
    [Required(ErrorMessage = "El usuario o email es obligatorio.")]
    [Display(Name = "Usuario o email")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Recordar sesión")]
    public bool RememberMe { get; set; }

    /// <summary>Ruta local a la que volver después de autenticar correctamente.</summary>
    public string? ReturnUrl { get; set; }

    public bool ShowLoggedOutMessage { get; set; }
}
