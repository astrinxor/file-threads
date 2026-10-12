namespace DragTrans.Domain.Entities;

public class SettingProfile
{
    public Guid UserId { get; set; }
    public string Description { get; set; } = "описание профиля";
    public string AvatarFilePath { get; set; } = string.Empty;
    public ICollection<StoredFile> Files { get; set; } = new List<StoredFile>();
    public bool IsOnline { get; set; } = false;
    public User User { get; set; } = null!;
}