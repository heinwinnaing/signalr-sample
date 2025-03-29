namespace SignalRApp.Entities;

public class User
    : Entity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Avator { get; set; } = "https://e7.pngegg.com/pngimages/799/987/png-clipart-computer-icons-avatar-icon-design-avatar-heroes-computer-wallpaper-thumbnail.png";
}
