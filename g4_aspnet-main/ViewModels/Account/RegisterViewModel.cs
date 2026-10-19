using System.ComponentModel.DataAnnotations;

namespace G4AspNet.ViewModels.Account;

/// <summary>Datos mínimos para crear una cuenta propia.</summary>
public sealed class RegisterViewModel
{
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    [StringLength(50, MinimumLength = 3)]
    [Display(Name = "Usuario")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "Escribe un email válido.")]
    public string Email { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "Nombre visible")]
    public string? DisplayName { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirma la contraseña.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
    [Display(Name = "Confirmar contraseña")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
