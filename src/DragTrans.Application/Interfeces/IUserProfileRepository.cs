using DragTrans.Domain.Entities;

namespace DragTrans.Application.Interfaces;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetProfileByUserIdAsync(Guid userId);
    Task UpdateDescriptionAsync(string description, Guid userId);
    Task UpdateAvatarFilePathAsync(string avatarFilePath, Guid userId);
    Task AddDefaultAsync(UserProfile profile, Guid userId);
    Task UpdateOnlineStatusAsync(Guid userId, bool isOnline);
}