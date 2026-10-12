using DragTrans.Domain.Entities;

namespace DragTrans.Application.Interfaces;

    public interface ISettingProfileRepository
{
    Task<SettingProfile?> GetProfileByUserIdAsync(Guid userId);
    Task UpdateDescriptionAsync(string description, Guid userId);
    Task UpdateAvatarFilePathAsync(string avatarFilePath, Guid userId);
    Task UpdateOnlineStatusAsync(Guid userId, bool isOnline);
    Task<string> CreateDefaultAvatarAsync(Guid userId);
}