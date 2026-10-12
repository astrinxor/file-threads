namespace DragTrans.Application.Services.Register;

public class RegisterResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime CreateTime { get; set; }
    public string Token { get; set; } = string.Empty;
}
