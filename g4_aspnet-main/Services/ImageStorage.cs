using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace G4AspNet.Services;

/// <summary>
/// Almacenamiento local sencillo para el aula. Se valida extensión, tamaño y firma;
/// SQLite solo conserva el nombre aleatorio generado por la aplicación.
/// </summary>
public sealed class ImageStorage
{
    private static readonly byte[] PngSignature = [137, 80, 78, 71, 13, 10, 26, 10];

    private readonly string webRootPath;
    private readonly ImageStorageOptions options;
    private readonly ILogger<ImageStorage> logger;

    public ImageStorage(
        IWebHostEnvironment environment,
        IOptions<ImageStorageOptions> options,
        ILogger<ImageStorage> logger)
    {
        webRootPath = environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot");
        this.options = options.Value;
        this.logger = logger;
    }

    /// <summary>Valida y guarda el avatar con un nombre nuevo, sin usar el del navegador.</summary>
    public ImageUploadResult Save(IFormFile file)
    {
        if (file.Length <= 0)
        {
            return ImageUploadResult.Failure("Selecciona una imagen.");
        }

        if (file.Length > options.MaxFileSizeBytes)
        {
            return ImageUploadResult.Failure("La imagen no puede superar 5 MB.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!options.AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
        {
            return ImageUploadResult.Failure("Solo se permiten imágenes JPG, PNG, GIF o WEBP.");
        }

        using var input = file.OpenReadStream();
        var header = new byte[12];
        var bytesRead = input.Read(header, 0, header.Length);
        if (!LooksLikeSupportedImage(header, bytesRead, extension))
        {
            return ImageUploadResult.Failure("El contenido del archivo no parece una imagen válida.");
        }

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var path = Path.Combine(GetDirectory(), fileName);
        Directory.CreateDirectory(GetDirectory());

        try
        {
            using var output = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            output.Write(header, 0, bytesRead);
            input.CopyTo(output);
        }
        catch (IOException exception)
        {
            TryDelete(path);
            logger.LogError(exception, "No se pudo guardar el avatar {Path}", path);
            return ImageUploadResult.Failure("No se pudo guardar la imagen. Inténtalo de nuevo.");
        }

        return ImageUploadResult.Success(fileName);
    }

    /// <summary>Elimina un avatar solo si su nombre no permite salir de la carpeta prevista.</summary>
    public void Delete(string? fileName)
    {
        if (!IsSafeFileName(fileName))
        {
            return;
        }

        TryDelete(Path.Combine(GetDirectory(), fileName!));
    }

    /// <summary>Construye la URL pública de un avatar guardado de forma segura.</summary>
    public string? GetUrl(string? fileName)
    {
        if (!IsSafeFileName(fileName))
        {
            return null;
        }

        return $"/uploads/avatars/{Uri.EscapeDataString(fileName!)}";
    }

    private string GetDirectory() => Path.Combine(webRootPath, "uploads", "avatars");

    private static bool IsSafeFileName(string? fileName) =>
        !string.IsNullOrWhiteSpace(fileName) &&
        string.Equals(Path.GetFileName(fileName), fileName, StringComparison.Ordinal);

    private static bool LooksLikeSupportedImage(byte[] header, int length, string extension)
    {
        if (extension is ".jpg" or ".jpeg")
        {
            return length >= 3 && header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF;
        }

        if (extension == ".png")
        {
            return length >= PngSignature.Length && header[..PngSignature.Length].SequenceEqual(PngSignature);
        }

        if (extension == ".gif")
        {
            return length >= 6 && (Encoding.ASCII.GetString(header, 0, 6) is "GIF87a" or "GIF89a");
        }

        return extension == ".webp" && length >= 12 &&
            Encoding.ASCII.GetString(header, 0, 4) == "RIFF" &&
            Encoding.ASCII.GetString(header, 8, 4) == "WEBP";
    }

    private void TryDelete(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (IOException exception)
        {
            logger.LogWarning(exception, "No se pudo eliminar el avatar {Path}", path);
        }
    }
}
