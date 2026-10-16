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

    public async Task<Profile?> GetProfileAsync(Guid userId)
    {
        return await _settingProfileRepository.GetProfileByUserIdAsync(userId);
    }

    public async Task UpdateDescriptionAsync(string newDescription, Guid userId)
    {
        await _settingProfileRepository.UpdateDescriptionAsync(newDescription, userId);
    }

    public async Task UpdateOnlineStatusAsync(Guid userId, bool isOnline)
    {
        await _settingProfileRepository.UpdateOnlineStatusAsync(userId, isOnline);
    }
}