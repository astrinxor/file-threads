using DragTrans.Domain.Entities;

namespace DragTrans.Application.Interfaces;

    public interface ISettingProfileRepository
{
    Task<Profile?> GetProfileByUserIdAsync(Guid userId);
    Task UpdateDescriptionAsync(string newDescription, Guid userId);
    Task UpdateOnlineStatusAsync(Guid userId, bool isOnline);
    Task<string> CreateDefaultAvatarAsync(Guid userId);
}