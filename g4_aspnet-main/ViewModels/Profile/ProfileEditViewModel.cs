using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace G4AspNet.ViewModels.Profile;

/// <summary>Campos que una persona puede editar de su propia cuenta.</summary>
public sealed class ProfileEditViewModel
{
    [Required(ErrorMessage = "El nombre visible es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Nombre visible")]
    public string DisplayName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "Escribe un email válido.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Avatar")]
    public IFormFile? Avatar { get; set; }

    public string? CurrentAvatarFileName { get; set; }

    [Display(Name = "Eliminar avatar actual")]
    public bool RemoveAvatar { get; set; }
}
