using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SignalRApp.Entities;
using SignalRApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Context>(options =>
{
    options.UseInMemoryDatabase("db_inMemory");
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/";
        options.Cookie.Name = "localhost";
    });
builder.Services.AddSignalR();
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.UseEndpoints(endpoints => 
{
    endpoints.MapHub<MessageHub>("/hub/message");
});

#region users
using(var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Context>();
    var users = new List<User>
    {
        new User{ Id = new Guid("53ac4dd9-84df-42c4-a5ea-d14a0ce65493"), Name = "Hein Win Naing", Email = "heinwinnaing@gmail.com" },
        new User{ Id = new Guid("3da4c3a5-93a9-4c7b-b4a3-f9e5d6612564"), Name = "John Smith", Email = "johnsmith@mailinator.com" },
        new User{ Id = new Guid("59819c02-840a-492c-bbfc-3a9bf6e995f9"), Name = "William", Email = "william@mailinator.com" },
    };
    context
        .Users
        .AddRange(users);
    context.SaveChanges();
}
#endregion

app.Run();
