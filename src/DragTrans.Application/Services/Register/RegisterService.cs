using DragTrans.Application.Interfaces;
using DragTrans.Domain.Entities;
using System.Data;
using DragTrans.Application.Services.Jwt;
using Microsoft.Extensions.Configuration;

namespace DragTrans.Application.Services.Register;

public class RegisterService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly ISettingProfileRepository _profileStorage;
    private readonly IConfiguration _configuration;

    public RegisterService(IUserRepository userRepository, IJwtService jwtService, 
    ISettingProfileRepository profileStorage, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _profileStorage = profileStorage;
        _configuration = configuration;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        var exitingUser = await _userRepository.GetByUserNameAsync(request.UserName);
        if (exitingUser != null)
        {
            throw new Exception("Это имя пользователя уже используется");
        }
        
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var userId = Guid.NewGuid();
        var avatarId = Guid.NewGuid();
        var avatarContentType = "image/png";
        
        string rootPath = _configuration["Storage:RootPath"];
        string userDirectoryPath = Path.Combine(rootPath, userId.ToString());
        string UserAvatarPath = Path.Combine(userDirectoryPath, $"{avatarId}.png");
        Directory.CreateDirectory(userDirectoryPath);
        string sourcePath = Path.Combine(_configuration["Storage:DefaultAvatarPath"], "avatar.png");

        File.Copy(sourcePath, UserAvatarPath);

        var fileInfo = new FileInfo(UserAvatarPath);
        long avatarSize = fileInfo.Length;
        

        var user = new User
        {
            Id = userId,
            Email = request.Email,
            UserName = request.UserName,
            PasswordHash = passwordHash,
            CreateTime = DateTime.UtcNow,
            Profile = new global::DragTrans.Domain.Entities.Profile
            {
                UserId = userId,
                Description = "описание профиля",
                IsOnline = false,
                AvatarId = avatarId,
                Avatar = new global::DragTrans.Domain.Entities.Avatar
                {
                    Id = avatarId,
                    ContentType = avatarContentType,
                    Size = avatarSize
                }
            }
        };
        
        await _userRepository.AddAsync(user);
        var token = _jwtService.GenerateToken(user.Id, user.UserName);

        return new RegisterResponse
        {
            Email = user.Email,
            Id = user.Id,
            UserName = user.UserName,
            CreateTime = user.CreateTime,
            Token = token
        };
    }
}
