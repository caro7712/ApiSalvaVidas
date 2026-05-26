using ApiSalvarVidas.Compartidos.DTOs;
using ApiSalvarVidas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiSalvaVidas.Controllers;

/// <summary>
/// Endpoint seguro para subir imágenes a Cloudinary.
///
/// El Front NUNCA toca Cloudinary directamente.
/// El ApiSecret vive SOLO aquí, en el backend (appsettings.json).
///
/// Flujo:
///   Front → POST api/cloudinary/upload (multipart)
///   → CloudinaryController
///   → CloudinaryService
///   → CloudinaryRepository (SDK CloudinaryDotNet)
///   → Cloudinary
///   ← secure_url ← Front
/// </summary>
[ApiController]
[Route("api/cloudinary")]
[Authorize]
public class CloudinaryController : ControllerBase
{
    private readonly CloudinaryService _service;

    public CloudinaryController(CloudinaryService service)
    {
        _service = service;
    }

    /// <summary>
    /// Recibe el archivo (multipart/form-data) y lo sube a Cloudinary.
    /// Retorna CloudinaryUploadResultDto con la URL pública segura.
    /// </summary>
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<CloudinaryUploadResultDto>> SubirImagen(
        [FromForm] CloudinaryUploadRequestDto request)
    {
        if (request.Archivo == null || request.Archivo.Length == 0)
            return BadRequest("Debe enviarse un archivo.");

        if (string.IsNullOrWhiteSpace(request.Carpeta))
            return BadRequest("La carpeta es obligatoria.");

        var resultado = await _service.SubirImagenAsync(
            request.Archivo,
            request.Carpeta);

        if (resultado == null)
            return BadRequest("No se pudo subir la imagen a Cloudinary.");

        return Ok(resultado);
    }
}
