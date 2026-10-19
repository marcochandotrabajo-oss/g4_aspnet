using System.ComponentModel.DataAnnotations;
using G4AspNet.Models;

namespace G4AspNet.ViewModels.Users;

/// <summary>Campos que un administrador puede modificar de una cuenta existente.</summary>
public sealed class EditUserViewModel
{
    [Required]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = "Nombre visible")]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Nueva contraseña (opcional)")]
    [StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [Required]
    public string Role { get; set; } = RoleNames.User;

    [Display(Name = "Cuenta activa")]
    public bool IsActive { get; set; }

    public string? CurrentAvatarFileName { get; set; }
}
