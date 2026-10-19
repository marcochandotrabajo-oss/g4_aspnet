using System.ComponentModel.DataAnnotations;

namespace G4AspNet.ViewModels.Profile;

/// <summary>Formulario de cambio de contraseña de la cuenta autenticada.</summary>
public sealed class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña actual")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "Nueva contraseña")]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Las contraseñas no coinciden.")]
    [Display(Name = "Confirmar nueva contraseña")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
