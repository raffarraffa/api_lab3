//using ImageMagick;
namespace api.Services;

public static class Utils
{
    public static bool IsImageValid(IFormFile file, int maxFileSize = 2048 * 1024)
    {
        string[] allowedContentTypes = { "image/jpeg", "image/png", "image/webp" };
        if (!allowedContentTypes.Contains(file.ContentType)) return false;
        if (file.Length > maxFileSize) return false;
        return true;
    }
    //  public static void RedimensionarImagen(IFormFile file, string outputPath, int maxWidth, int maxHeight)
    //     {
    //         // Validar el archivo de entrada
    //         if (file == null || file.Length == 0)
    //         {
    //             throw new ArgumentException("El archivo no puede ser nulo o vacío.", nameof(file));
    //         }

    //         using (var stream = new MemoryStream())
    //         {
    //             // Copiar el archivo al MemoryStream
    //             file.CopyTo(stream);
    //             stream.Position = 0; // Reiniciar el flujo

    //             // Cargar la imagen desde el flujo
    //             using (var image = new MagickImage(stream))
    //             {
    //                 // Redimensionar manteniendo la relación de aspecto
    //                 image.Resize(new MagickGeometry(maxWidth, maxHeight) { IgnoreAspectRatio = false });

    //                 // Guardar la imagen en el formato especificado
    //                 image.Write(outputPath); // Guardar el archivo en la ruta de salida
    //             }
    //         }
    //     }
}



