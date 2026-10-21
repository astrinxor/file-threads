using DragTrans.Application.Service.SettingProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DragTrans.Application.Services.SettingProfile;
using DragTrans.Domain.Entities;

namespace DragTrans.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly ProfileService _profileservice;

    public ProfileController(ProfileService profileservice)
    {
        _profileservice = profileservice;
    }

    [HttpGet("getavatar")]
    public async Task<IActionResult> GetAvatar()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var avatar = await _profileservice.ProfileAvatarGetAsync(userId);

        return Ok(avatar);
    } 

    [HttpPut("change")]
    public async Task<IActionResult> ChangeAvatar(IFormFile avatar)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var storedAvatar = await _profileservice.ReplacementAvatar(avatar, userId);

        return Ok(storedAvatar);
    }

}