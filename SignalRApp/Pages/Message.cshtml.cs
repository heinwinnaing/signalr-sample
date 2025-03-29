using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRApp.Entities;
using System.Security.Claims;

namespace SignalRApp.Pages
{
    public class MessageModel : PageModel
    {
        private readonly Context _context;

        public MessageModel(Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGet()
        {
            if (User?.Identity?.IsAuthenticated == false)
            {
                return LocalRedirect("/");
            }
            _ = Guid.TryParse(User!.Claims.FirstOrDefault(r=> r.Type == ClaimTypes.NameIdentifier)?.Value, out Guid userId);
            return Page();
        }
    }
}
