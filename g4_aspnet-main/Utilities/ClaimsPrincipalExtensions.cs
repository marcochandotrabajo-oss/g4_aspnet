using System.Security.Claims;

namespace G4AspNet.Utilities;

/// <summary>Atajos legibles para obtener datos de la cuenta autenticada.</summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Obtiene el ID que Identity guardó en la cookie. Las acciones que lo usan ya
    /// tienen <c>[Authorize]</c>; si faltase la claim sería un error de configuración.
    /// </summary>
    public static string GetRequiredUserId(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("La sesión autenticada no contiene el identificador del usuario.");
}
