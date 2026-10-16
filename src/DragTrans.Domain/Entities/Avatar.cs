namespace DragTrans.Domain.Entities;

public class Avatar
{
    public Guid Id { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
}