namespace SignalRApp.Entities;

public class Message
    : Entity
{
    public Guid SenderId { get; set; }
    public string Text { get; set; } = null!;
    public virtual User Sender { get; set; } = null!;
}