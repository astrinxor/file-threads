using DragTrans.Infrastructure.Data;
using DragTrans.Application.Interfaces;
using DragTrans.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DragTrans.Infrastructure.Repositories;

public class SettingProfileRepository : ISettingProfileRepository
{
    private readonly DragTransDbContext _dbContext;
    private readonly string _storageRoot;
    private readonly string _defaultAvatarPath;

    public SettingProfileRepository(DragTransDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;

        // Переходим от bin/Debug/net10.0 к корню FileThread.
        string storageBase = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "..", ".."));

        _storageRoot = Path.GetFullPath(Path.Combine(storageBase, configuration["Storage:RootPath"]
                ?? throw new InvalidOperationException("Storage:RootPath не задан.")));

        _defaultAvatarPath = Path.GetFullPath(Path.Combine(storageBase, configuration["Storage:DefaultAvatarPath"]
                ?? throw new InvalidOperationException("Storage:DefaultAvatarPath не задан."))
);
    }

    public async Task<SettingProfile?> GetProfileByUserIdAsync(Guid userId)
    {
        return await _dbContext.SettingProfiles
            .FirstOrDefaultAsync(profile => profile.UserId == userId);
    }
    public async Task UpdateDescriptionAsync(string newDescription, Guid userId)
    {
        var profile = await _dbContext.SettingProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile != null)
        {
            profile.Description = newDescription;
            await _dbContext.SaveChangesAsync();
        }
        
    }

    public async Task UpdateAvatarFilePathAsync(string avatarFilePath, Guid userId)
    {
        var profile = await _dbContext.SettingProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile != null)
        {
            profile.AvatarFilePath = avatarFilePath;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateOnlineStatusAsync(Guid userId, bool isOnline)
    {
        var profile = await _dbContext.SettingProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile != null)
        {
            profile.IsOnline = isOnline;
            await _dbContext.SaveChangesAsync();
        }
    }

    public Task<string> CreateDefaultAvatarAsync(Guid userId)
    {
        string profileDirectory = Path.Combine(_storageRoot, "Profiles", userId.ToString());

        Directory.CreateDirectory(profileDirectory);

        string destinationPath = Path.Combine(profileDirectory, "avatar.png");

        File.Copy(_defaultAvatarPath, destinationPath, overwrite: true);

        return Task.FromResult(Path.Combine("Profiles", userId.ToString(), "avatar.png").Replace('\\', '/'));
    }
    
}
