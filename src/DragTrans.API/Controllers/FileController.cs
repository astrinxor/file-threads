using DragTrans.Application.Services.Files;
using DragTrans.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DragTrans.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FileController : ControllerBase
{
    private readonly FileService _fileservice;
    private readonly IFileRepository _filerepository;

    public FileController(FileService fileservice, IFileRepository filerepository)
    {
        _fileservice = fileservice;
        _filerepository = filerepository;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    { 
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var storedFile = await _fileservice.UploadAsync(file, userId);

        return Ok(storedFile);
    }

    
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

    [HttpGet("getAll")]

    public async Task<IActionResult> getall()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var fileses = await _filerepository.GetByUserIdAsync(userId);
        return Ok(fileses);
    }

}