namespace G4AspNet.Models;

/// <summary>Datos mínimos que muestra la página de error controlada.</summary>
public sealed class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
