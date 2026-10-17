using DragTrans.Application.Interfaces;
using DragTrans.Domain.Entities;

namespace DragTrans.Application.Service.SettingProfile;

public class ProfileService
{
    private readonly ISettingProfileRepository _settingProfileRepository;

    public ProfileService(ISettingProfileRepository settingProfileRepository)
    {
        _settingProfileRepository = settingProfileRepository;
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
}