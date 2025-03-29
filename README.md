# What is SignalR?
SignalR is a real-time communication library in ASP.NET Core that enables bi-directional communication between servers and clients. It allows web applications to push updates to clients instantly without requiring clients to constantly poll the server.

## Key Features of SignalR
<ul>
  <li>Real-Time Communication: Pushes messages instantly from server to clients.</li>
  <li>Supports Multiple Transport Methods: Uses WebSockets, Server-Sent Events (SSE), or Long Polling based on client capability.</li>
  <li>Automatic Reconnection: Handles dropped connections and reconnects automatically.</li>
  <li>Scalability with Backplane: Supports Redis, Azure SignalR Service, etc., for scaling.</li>
  <li>Group & User Messaging: Allows sending messages to specific users or groups.</li>
</ul>

## Install SignalR Package
```
dotnet add package Microsoft.AspNetCore.SignalR
```

## A Hub manages real-time client-server communication.
```csharp
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
```
Clients.All.SendAsync("ReceiveMessage", user, message) → Sends a message to all connected clients.

## Configure SignalR in Program.cs
```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR(); // Add SignalR Service

var app = builder.Build();
app.MapHub<ChatHub>("/chatHub"); // Map the SignalR Hub

app.Run();
```
app.MapHub<ChatHub>("/chatHub") → Exposes the SignalR endpoint at /chatHub.
