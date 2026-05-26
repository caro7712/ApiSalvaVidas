using ApiSalvarVidas.Compartidos.DTOs;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ApiSalvarVidas.Repositories;

public class CloudinaryRepository
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryRepository(
        IConfiguration config)
    {
        var cloudName =
            config["CloudinarySettings:CloudName"];

        var apiKey =
            config["CloudinarySettings:ApiKey"];

        var apiSecret =
            config["CloudinarySettings:ApiSecret"];

        var account = new Account(
            cloudName,
            apiKey,
            apiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<CloudinaryUploadResultDto?>
        SubirImagenAsync(
            Stream stream,
            string nombreArchivo,
            string carpeta)
    {
        // =====================================================
        // PARAMS
        // =====================================================

        var uploadParams =
            new ImageUploadParams
            {
                File = new FileDescription(
                    nombreArchivo,
                    stream
                ),

                Folder = carpeta,

                UseFilename = true,

                UniqueFilename = true,

                Overwrite = false
            };

        // =====================================================
        // UPLOAD SEGURO
        // =====================================================

        var result =
            await _cloudinary
                .UploadAsync(uploadParams);

        // =====================================================
        // VALIDAR RESULTADO
        // =====================================================

        if (result == null ||
            result.SecureUrl == null)
        {
            return null;
        }

        // =====================================================
        // DTO RESPONSE
        // =====================================================

        return new CloudinaryUploadResultDto
        {
            Url = result.SecureUrl.ToString(),

            PublicId = result.PublicId ?? "",

            Carpeta = carpeta
        };
    }
}