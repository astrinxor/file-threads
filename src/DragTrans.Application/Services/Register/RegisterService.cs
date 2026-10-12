using DragTrans.Application.Interfaces;
using DragTrans.Domain.Entities;
using System.Data;
using DragTrans.Application.Services.Jwt;

namespace DragTrans.Application.Services.Register;

public class RegisterService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly ISettingProfileRepository _profileStorage;

    public RegisterService(IUserRepository userRepository, IJwtService jwtService, ISettingProfileRepository profileStorage)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _profileStorage = profileStorage;
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

        var user = new User
        {
            Id = userId,
            Email = request.Email,
            UserName = request.UserName,
            PasswordHash = passwordHash,
            CreateTime = DateTime.UtcNow,
            Profile = new global::DragTrans.Domain.Entities.SettingProfile
            {
                Description = "описание профиля",
                AvatarFilePath = Path.Combine("Profiles", userId.ToString(), "avatar.png"),
                IsOnline = false
            }
        };

        user.Profile.UserId = user.Id;
        
        await _userRepository.AddAsync(user);
        var token = _jwtService.GenerateToken(user.Id, user.UserName);
        string avatarPath = await _profileStorage.CreateDefaultAvatarAsync(user.Id);

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
