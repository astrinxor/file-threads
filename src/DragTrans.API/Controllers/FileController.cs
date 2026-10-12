using DragTrans.Application.Services.Files;
using DragTrans.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DragTrans.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly FileService _fileservice;
    private readonly IFileRepository _filerepository;

    public FileController(FileService fileservice, IFileRepository filerepository)
    {
        _fileservice = fileservice;
        _filerepository = filerepository;
    }

    [Authorize]
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    { 
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var storedFile = await _fileservice.UploadAsync(file, userId);

        return Ok(storedFile);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        if (!Guid.TryParse(userIdClaim, out Guid userId))
            return Unauthorized();

        try
        {
            await _fileservice.DeleteAsync(id, userId);

            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Файл не найден.");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [Authorize]
    [HttpGet("getAll")]

    public async Task<IActionResult> getall(Guid userId)
    {
        var fileses = await _filerepository.GetByUserIdAsync(userId);
        return Ok(fileses);
    }

}