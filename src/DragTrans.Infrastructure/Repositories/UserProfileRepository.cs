
using DragTrans.Infrastructure.Data;
using DragTrans.Application.Interfaces;
using DragTrans.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DragTrans.Infrastructure.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly DragTransDbContext _dbContext;

    public UserProfileRepository(DragTransDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<UserProfile?> GetProfileByUserIdAsync(Guid userId)
    {
        return await _dbContext.UserProfiles.FirstOrDefaultAsync(profile => profile.UserId == userId);
    }

    public async Task UpdateDescriptionAsync(string description, Guid userId)
    {
        await _dbContext.UserProfiles.Where(profile => profile.UserId == userId).ExecuteUpdateAsync(x => x.SetProperty(profile => profile.Description, description));
    }

    public async Task UpdateAvatarFilePathAsync(string avatarFilePath, Guid userId)
    {
        await _dbContext.UserProfiles.Where(profile => profile.UserId == userId).ExecuteUpdateAsync(x => x.SetProperty(profile => profile.AvatarFilePath, avatarFilePath));
    }

    public async Task AddDefaultAsync(UserProfile profile, Guid userId)
    {
        await _dbContext.UserProfiles.AddAsync(profile);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateOnlineStatusAsync(Guid userId, bool isOnline)
    {
        await _dbContext.UserProfiles.Where(profile => profile.UserId == userId).ExecuteUpdateAsync(x => x.SetProperty(profile => profile.IsOnline, isOnline));
    }
}
