namespace G4AspNet.Services;

/// <summary>Resultado de guardar una imagen, con un error que se puede mostrar en un formulario.</summary>
public sealed record ImageUploadResult(string? FileName = null, string? Error = null)
{
    public bool Succeeded => FileName is not null;

    public static ImageUploadResult Success(string fileName) => new(fileName);

    public static ImageUploadResult Failure(string error) => new(Error: error);
}
