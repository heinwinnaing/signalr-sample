using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SignalRApp.Entities;
using System.Security.Claims;

namespace SignalRApp.Hubs;

public class MessageHub
    : BaseHub
{
    public MessageHub(Context context) 
        : base(context)
    {
    }

    public async Task SendMessage(SendMessage msg)
    {
        _ = Guid.TryParse(Context.UserIdentifier, out Guid senderId);
        var sender = await _context
            .Users
            .FirstOrDefaultAsync(r => r.Id == senderId);
        
        if(sender is not null)
        {
            var message = new Message 
            {
                Id = Guid.NewGuid(),
                SenderId = senderId,
                Text = msg.message
            };
            await _context
                .Messages
                .AddAsync(message);
            await _context.SaveChangesAsync();

            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveMessage", new
            {
                message.Id,
                message.Text,
                message.CreatedAt,
                Sender = new { sender.Id, sender.Name, sender.Email, sender.Avator }
            });
        }
    }
}
