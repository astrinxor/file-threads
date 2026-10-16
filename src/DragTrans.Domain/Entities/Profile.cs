namespace DragTrans.Domain.Entities;

public class Profile
{
    public Guid UserId { get; set; }
    public Guid AvatarId { get; set; }
    public string Description { get; set; } = "описание профиля";
    public ICollection<StoredFile> Files { get; set; } = new List<StoredFile>();
    public Avatar Avatar { get; set; } = null!;
    public bool IsOnline { get; set; } = false;
    public User User { get; set; } = null!;
}