using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRApp.Entities;
using System.Security.Claims;

namespace SignalRApp.Hubs;

public class BaseHub
    : Hub
{
    protected readonly Context _context;
    public BaseHub(Context context)
    {
        _context = context;
    }

    #region OnConnectedAsync
    public override async Task OnConnectedAsync()
    {
        if (Context.User?.Identity?.IsAuthenticated == false)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("Connected", "Unable to connect service");
        }
        else
        {
            _= Guid.TryParse(Context.UserIdentifier, out Guid userIdClaim);
            var name = Context.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = Context.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            var member = await _context
                .Users
                .FirstOrDefaultAsync(r => r.Id == userIdClaim);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("Connected", new
            {
                id = member.Id,
                name = member.Name,
                email = member.Email,
                avator = member.Avator
            });
            await base.OnConnectedAsync();
        }
    }
    #endregion

    #region OnDisconnectedAsync
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try 
        {
            _ = Guid.TryParse(Context.UserIdentifier, out Guid userIdClaim);
        }
        catch(Exception ex) 
        {
            Console.WriteLine(ex);
        }
        await base.OnDisconnectedAsync(exception);
    }
    #endregion
}

public record SendMessage(string message)
{
    public Guid id { get; set; }
}

public record ReceiveMessage(string message)
{
    public Guid id { get; set; }
    public DateTime dt { get; set; }
    public string? avator { get; set; } = "https://e7.pngegg.com/pngimages/799/987/png-clipart-computer-icons-avatar-icon-design-avatar-heroes-computer-wallpaper-thumbnail.png";
}