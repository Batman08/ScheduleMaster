using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SM.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            //check if user is logged in redirect to user dashboard
            if (User.Identity!.IsAuthenticated && User.Identity != null)
            {
                return LocalRedirect(Url.Content("~/Dashboard/Home"));
            }
            return Page();
        }
    }
}