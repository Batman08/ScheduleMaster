using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SM.Database;
using SM.Engine.DashboardModule;
using System.Text.Json;

namespace SM.Web.Pages.Dashboard
{
    [Authorize]
    public class HomeModel : PageModel
    {
        private ISmRepository _smRepository;
        private HomeModule _homeModule;

        public HomeModel(ISmRepository smRepository)
        {
            _smRepository = smRepository;
            _homeModule = new HomeModule(_smRepository);
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public JsonResult OnPostCreateEvent([FromBody] SmEventItemDTO smData)
        {
            smData.UserId = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value.ToString());
            var result = _homeModule.SmSaveUserEvent(smData);
            return new JsonResult(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public JsonResult OnPostUpdateEvent([FromBody] SmEventItemUpdateDTO smEventData)
        {
            smEventData.UserId = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value.ToString());
            var result = _homeModule.SmUpdateUserEvent(smEventData);
            return new JsonResult(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public JsonResult OnPostDeleteEvent([FromBody] SmEventItemIdDTO smEventData)
        {
            smEventData.UserId = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value.ToString());

            var message = _homeModule.SmDeleteUserEvent(smEventData);
            return new JsonResult(new SmMessageItemDTO { Message = message }, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public JsonResult OnPostLoadEvents()
        {
            var userId = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value.ToString());
            return new JsonResult(_homeModule.SmLoadUserEvents(userId), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public JsonResult OnPostLoadEvent([FromBody] SmEventItemIdDTO smEventData)
        {
            smEventData.UserId = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value.ToString());
            return new JsonResult(_homeModule.SmLoadUserEvent(smEventData), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
