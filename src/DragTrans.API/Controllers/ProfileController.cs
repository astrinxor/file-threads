using DragTrans.Application.Service.Profile;
using Microsoft.AspNetCore.Authorization;
using DragTrans.Application.Services.Profile.ChangeDescription;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DragTrans.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly ProfileService _profileService;

    public ProfileController(ProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var id))
        {
            return Unauthorized();
        }

        var profile = await _profileService.GetProfileAsync(id);

        if (profile == null)
        {
            return NotFound();
        }

        return Ok(profile);
    }

    [HttpPut("ChangeDescription")]
    public async Task<IActionResult> ChangeDescription([FromBody] ChangeDescriptionRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var id))
        {
            return Unauthorized();
        }

        await _profileService.UpdateDescriptionAsync(request.NewDescription, id);
        return Ok();
    }

    [HttpPut("ChangeAvatar")]
    public async Task<IActionResult> ChangeAvatar()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var id))
        {
            return Unauthorized();
        }

        string AvatarPath = await _profileService.UpdateAvatarFilePathAsync(userId);
    }
}