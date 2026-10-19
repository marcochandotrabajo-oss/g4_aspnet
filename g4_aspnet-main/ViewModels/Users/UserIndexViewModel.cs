namespace G4AspNet.ViewModels.Users;

/// <summary>Filtro y resultados de la administración de usuarios.</summary>
public sealed class UserIndexViewModel
{
    public string? Search { get; init; }
    public List<UserListItemViewModel> Users { get; init; } = [];
}
