using DragTrans.Application.Interfaces;
using DragTrans.Domain.Entities;
using System.Data;
using DragTrans.Application.Services.Jwt;

namespace DragTrans.Application.Services.Register;

public class RegisterService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public RegisterService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        var exitingUser = await _userRepository.GetByUserNameAsync(request.UserName);
        if (exitingUser != null)
        {
            throw new Exception("Это имя пользователя уже используется");
        }
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            UserName = request.UserName,
            PasswordHash = passwordHash,
            CreateTime = DateTime.UtcNow,
            Profile = new UserProfile
            {
                Description = string.Empty,
                AvatarFilePath = string.Empty,
                IsOnline = false
            }
        };

        user.Profile.UserId = user.Id;
        
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
