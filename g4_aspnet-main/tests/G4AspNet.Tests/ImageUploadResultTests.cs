using G4AspNet.Services;

namespace G4AspNet.Tests;

/// <summary>Ejemplo mínimo de test unitario de una regla común de la plantilla.</summary>
public sealed class ImageUploadResultTests
{
    [Fact]
    public void Failure_DoesNotReportASuccessfulUpload()
    {
        var result = ImageUploadResult.Failure("Archivo no válido.");

        Assert.False(result.Succeeded);
        Assert.Null(result.FileName);
        Assert.Equal("Archivo no válido.", result.Error);
    }
}
