using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Repositories;

namespace ApiSalvarVidas.Services;

public class CloudinaryService
{
    private readonly CloudinaryRepository _repository;

    public CloudinaryService(
        CloudinaryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CloudinaryUploadResultDto?>
        SubirImagenAsync(
            IFormFile archivo,
            string carpeta)
    {
        // =====================================================
        // VALIDACIONES
        // =====================================================

        if (archivo == null || archivo.Length == 0)
            return null;

        if (string.IsNullOrWhiteSpace(carpeta))
            return null;

        // =====================================================
        // STREAM
        // =====================================================

        using var stream =
            archivo.OpenReadStream();

        return await _repository
            .SubirImagenAsync(
                stream,
                archivo.FileName,
                carpeta
            );
    }
}