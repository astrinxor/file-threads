namespace DragTrans.Application.Services.Profile;

public class ProfileRequest
{
    public Guid UserId { get; set; }
    public string? Description { get; set; } = "Описание профиля не установлено";
    public string? AvatarFilePath { get; set; } = "хуй";
}