namespace G4AspNet.Services;

/// <summary>Opciones de tamaño y extensiones permitidas para las imágenes subidas.</summary>
public sealed class ImageStorageOptions
{
    public long MaxFileSizeBytes { get; set; } = 5 * 1024 * 1024;

    public string[] AllowedExtensions { get; set; } =
    [".jpg", ".jpeg", ".png", ".gif", ".webp"];
}
