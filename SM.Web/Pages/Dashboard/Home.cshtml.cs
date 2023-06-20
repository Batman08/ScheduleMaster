using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SM.Engine.Dashboard;
using System.Text.Json;

namespace SM.Web.Pages.Dashboard
{
    public class HomeModel : PageModel
    {
        public void OnGet()
        {
        }

        public JsonResult OnPostCreateEvent([FromBody] SmEventDataDTO smData)
        {
            return new JsonResult(smData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
