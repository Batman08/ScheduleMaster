using SM.Database;
using SM.Database.DashboardCommandsQueries;

namespace SM.Engine.DashboardModule
{
    public class HomeModule
    {
        private ISmRepository _smRepository;

        public HomeModule(ISmRepository smRepository)
        {
            _smRepository = smRepository;
        }

        public void SmSaveUserEvent(SmEventItemDTO smData)
        {
            var dataToSave = new SmEventDataDTO
            {
                UserId = new Guid("F5CF2364-5ACF-4360-B9C6-4BC9636B87BF"),
                Title = smData.Title,
                Info = smData.Info,
                Start = smData.Start,
                End = smData.End,
                Colour = smData.Colour
            };
            var dashboard = new DashboardCommandsQueries(_smRepository);
            dashboard.SaveEvent(dataToSave);
        }
    }
}
