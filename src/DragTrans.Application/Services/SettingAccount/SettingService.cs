using System;
using System.Collections.Generic;
using System.Text;
using DragTrans.Application.Interfaces;
using DragTrans.Application.Services.SettingProfile.ChangePassword;

namespace DragTrans.Application.Services.SettingProfile;

public class SettingService
{
    private readonly IUserRepository _userRepository;

    public SettingService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ChangePasswordResponse> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new Exception("Пользователь не найден");
        }

        if (user.PasswordHash == null)
        {
            throw new Exception("У пользователя PasswordHash == null");
        }
        bool passwordValid = BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash);
        if (!passwordValid)
        {
            return new ChangePasswordResponse
            {
                Success = false,
                Message = $"Старый пароль указан неверно, {newPassword}"
            };
        }

        string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.PasswordHash = newPasswordHash;

        await _userRepository.UpdateAsync(user);

        return new ChangePasswordResponse
        {
            Success = true,
            Message = "Пароль был изменён"
        };
    }


}
