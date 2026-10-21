using DragTrans.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using DragTrans.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IO;

namespace DragTrans.Application.Service.SettingProfile;

public class ProfileService
{
    private readonly ISettingProfileRepository _settingProfileRepository;
    private readonly IConfiguration _configuration;

    public ProfileService(ISettingProfileRepository settingProfileRepository, IConfiguration configuration)
    {
        _settingProfileRepository = settingProfileRepository;
        _configuration = configuration;
    }

    public async Task<Avatar?> ProfileAvatarGetAsync(Guid userId)
    {
        var profile = await _settingProfileRepository.ProfileGetByIdAsync(userId);

        if (profile == null)
        {
            throw new Exception("Профиль не найден");
        }

        var avatarId = profile.AvatarId;

        Avatar? avatar = await _settingProfileRepository.AvatarGetByIdAsync(avatarId);

        return avatar;
    }

    public async Task<Avatar?> ReplacementAvatar(IFormFile avatar, Guid userId)
    {
        if(avatar == null || avatar.Length == 0)
        {
            throw new ArgumentException("Ошибка при загрузки аватара");
        }


        var extension = Path.GetExtension(avatar.FileName).ToLowerInvariant();
        if (extension != ".png" && extension != ".jpg")
        {
            throw new ArgumentException("Загрузить можно только .png, или .jpg формат");
        }

        var profile = await _settingProfileRepository.ProfileGetByIdAsync(userId);

        if(profile == null)
        {
            throw new Exception("Ошибка чтения профиля");
        }

        var oldAvatarId = profile.AvatarId;
        var oldAvatar = await _settingProfileRepository.AvatarGetByIdAsync(oldAvatarId); 
        var oldAvatarPath = Path.Combine(_configuration["Storage:RootPath"], userId.ToString(), $"{oldAvatar.Id}{oldAvatar.ContentType}");

        var avatarId = Guid.NewGuid();
        var avatarName = avatarId.ToString();
        var avatarPath = Path.Combine(_configuration["Storage:RootPath"], userId.ToString(), avatarId.ToString());

        await using (var stream = new FileStream(avatarPath, FileMode.CreateNew))
        {
            await avatar.CopyToAsync(stream);
        }

        System.IO.File.Delete(oldAvatarPath);

        var storedAvatar = new StoredFile
        {
            Id = avatarId,
            ContentType = extension,
            Size = avatar.Length
        };

        return oldAvatar;
    }
}