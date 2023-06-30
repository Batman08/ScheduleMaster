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

        public IActionResult OnGet()
        {
            //check if user is authorized otherwise redirect to login page
            if (!User.Identity!.IsAuthenticated || User.Identity == null)
            {
                return LocalRedirect(Url.Content("~/Identity/Account/Login"));
            }
            return Page();
        }

        public JsonResult OnPostCreateEvent([FromBody] SmEventItemDTO smData)
        {
            smData.UserId = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value.ToString());
            _homeModule.SmSaveUserEvent(smData);
            return new JsonResult(smData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public JsonResult OnPostUpdateEvent([FromBody] SmEventItemUpdateDTO smEventData)
        {
            smEventData.UserId = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value.ToString());
            _homeModule.SmUpdateUserEvent(smEventData);
            return new JsonResult(smEventData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
