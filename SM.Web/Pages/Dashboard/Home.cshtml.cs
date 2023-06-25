using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SM.Database;
using SM.Engine.DashboardModule;
using System.Text.Json;

namespace SM.Web.Pages.Dashboard
{
    public class HomeModel : PageModel
    {
        private ISmRepository _smRepository;
        private HomeModule _homeModule;

        public HomeModel(ISmRepository smRepository)
        {
            _smRepository = smRepository;
            _homeModule = new HomeModule(_smRepository);
        }

        public void OnGet()
        {
        }

        public JsonResult OnPostCreateEvent([FromBody] SmEventItemDTO smData)
        {
            _homeModule.SmSaveUserEvent(smData);
            return new JsonResult(smData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public JsonResult OnPostLoadEvents()
        {
            //Request.Cookies["Key"]
            return new JsonResult(_homeModule.SmLoadUserEvents(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
