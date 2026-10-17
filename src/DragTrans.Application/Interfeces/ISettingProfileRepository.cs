using DragTrans.Domain.Entities;

namespace DragTrans.Application.Interfaces;

    public interface ISettingProfileRepository
{
    public Task<Avatar?> AvatarGetByIdAsync(Guid avatarId);
    public Task<Profile?> ProfileGetByIdAsync(Guid userId); 
}