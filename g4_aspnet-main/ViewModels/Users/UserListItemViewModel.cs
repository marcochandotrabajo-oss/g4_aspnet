using G4AspNet.Models;

namespace G4AspNet.ViewModels.Users;

/// <summary>Una fila del listado administrativo de cuentas.</summary>
public sealed class UserListItemViewModel
{
    public ApplicationUser User { get; init; } = null!;
    public string Role { get; init; } = RoleNames.User;
}
