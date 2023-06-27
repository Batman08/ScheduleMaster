using SM.Database;
using SM.Database.DashboardCommandsQueries;

namespace SM.Engine.DashboardModule
{
    public class HomeModule
    {
        private ISmRepository _smRepository;
        private DashboardCommandsQueries _dashboardCommandsQueries;

        public HomeModule(ISmRepository smRepository)
        {
            _smRepository = smRepository;
            _dashboardCommandsQueries = new DashboardCommandsQueries(_smRepository);
        }

        public void SmSaveUserEvent(SmEventItemDTO smData)
        {
            var dataToSave = new SmEventDataDTO
            {
                UserId = smData.UserId,
                Day = smData.Day,
                Title = smData.Title,
                Info = smData.Info,
                Start = smData.Start,
                End = smData.End,
                Colour = smData.Colour
            };
            _dashboardCommandsQueries.SaveEvent(dataToSave);
        }

        public List<SpGetUserEventsReturnModel> SmLoadUserEvents()
        {
            return _dashboardCommandsQueries.LoadEvents("f5cf2364-5acf-4360-b9c6-4bc9636b87bf");
        }
    }
}
