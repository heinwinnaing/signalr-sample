using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using SignalRApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace SignalRApp.Pages;

public class IndexModel : PageModel
{
    private readonly Context _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(Context context,
        ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        if(User?.Identity?.IsAuthenticated == true)
        {
            return LocalRedirect("/message");
        }
        await Task.Delay(1);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var name = Request.Form["name"];
            var email = Request.Form["email"];
            if (!string.IsNullOrEmpty(name)
                && !string.IsNullOrEmpty(email))
            {
                var user = await _context
                    .Users
                    .FirstOrDefaultAsync(r => r.Email == email);
                if(user is null)
                {
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        Name = name!,
                        Email = email!,
                        Avator = "https://e7.pngegg.com/pngimages/799/987/png-clipart-computer-icons-avatar-icon-design-avatar-heroes-computer-wallpaper-thumbnail.png",
                        CreatedAt = DateTime.UtcNow
                    };
                    await _context.Users.AddAsync(user);
                }
                else
                {
                    user.Name = name!;
                }
                await _context.SaveChangesAsync();
                var claims = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, $"{ user.Id }"),
                            new Claim(ClaimTypes.Name, $"{ user.Name }"),
                            new Claim(ClaimTypes.Email, $"{ user.Email }"),
                        }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(claims);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddYears(1),
                    RedirectUri = "/",
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
                return LocalRedirect("/message");
            }
        }
        catch (Exception ex)
        {

        }

        return Page();
    }
}
