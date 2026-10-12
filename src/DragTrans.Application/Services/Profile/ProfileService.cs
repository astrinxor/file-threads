using DragTrans.Application.Interfaces;
using DragTrans.Domain.Entities;

namespace DragTrans.Application.Service.Profile;

public class ProfileService
{
    private readonly IUserProfileRepository _userProfileRepository;

    public ProfileService(IUserProfileRepository userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }

    public async Task<UserProfile?> GetProfileAsync(Guid userId)
    {
        return await _userProfileRepository.GetProfileByUserIdAsync(userId);
    }

    public async Task UpdateDescriptionAsync(string description, Guid userId)
    {
        await _userProfileRepository.UpdateDescriptionAsync(description, userId);
    }
}