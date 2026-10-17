using DragTrans.Infrastructure.Data;
using DragTrans.Application.Interfaces;
using DragTrans.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace DragTrans.Infrastructure.Repositories;

public class SettingProfileRepository : ISettingProfileRepository
{

    private readonly DragTransDbContext _db;

    public SettingProfileRepository(DragTransDbContext db)
    {
        _db = db;
    }
    public async Task<Avatar?> AvatarGetByIdAsync(Guid avatarId)
    {
        var profileAvatar = await _db.Avatars.FirstOrDefaultAsync(avatar => avatar.Id == avatarId);

        return profileAvatar;
    }

    public async Task<Profile?> ProfileGetByIdAsync(Guid userId)
    {
        var profile = await _db.Profiles.FirstOrDefaultAsync(profile => profile.UserId == userId);
        return profile;
    }


}