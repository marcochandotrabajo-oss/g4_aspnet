using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using G4AspNet.Models;

namespace G4AspNet.ViewModels.Users;

/// <summary>Formulario de alta usado por un administrador.</summary>
public sealed class CreateUserViewModel
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    [Display(Name = "Usuario")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = "Nombre visible")]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
    [Display(Name = "Confirmar contraseña")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = RoleNames.User;

    [Display(Name = "Cuenta activa")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Avatar")]
    public IFormFile? Avatar { get; set; }
}
