using DragTrans.Application.Services.SettingProfile;
using DragTrans.Application.Services.SettingProfile.ChangePassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DragTrans.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingAccountController : ControllerBase
{
    private readonly SettingService _settingService;

    public SettingAccountController(SettingService settingService)
    {
        _settingService = settingService;
    }

    [Authorize]
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _settingService.ChangePasswordAsync(
            userId,
            request.OldPassword,
            request.NewPassword);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok();

    }
}
