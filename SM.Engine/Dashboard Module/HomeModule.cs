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
                EventDataId = smData.EventDataId,
                Day = smData.Day,
                Title = smData.Title,
                Info = smData.Info,
                Start = smData.Start,
                End = smData.End,
                Colour = smData.Colour
            };
            _dashboardCommandsQueries.SaveEvent(dataToSave);
        }

        public void SmUpdateUserEvent(SmEventItemUpdateDTO smUpdateData)
        {
            var dataToSave = new SmEventDataUpdateDTO
            {
                UserId = smUpdateData.UserId,
                EventDataId = smUpdateData.EventDataId,
                Day = smUpdateData.Day,
                Title = smUpdateData.Title,
                Info = smUpdateData.Info,
                Start = smUpdateData.Start,
                End = smUpdateData.End,
                Colour = smUpdateData.Colour
            };
            _dashboardCommandsQueries.UpdateEvent(dataToSave);
        }

        public string SmDeleteUserEvent(SmEventItemIdDTO smData)
        {
            var dataToSave = new SmEventDataIdDTO
            {
                UserId = smData.UserId,
                EventDataId = smData.EventDataId,
            };
            return _dashboardCommandsQueries.DeleteEvent(dataToSave).First().Message;
        }

        public List<SpGetUserEventsReturnModel> SmLoadUserEvents(Guid userId)
        {
            return _dashboardCommandsQueries.LoadEvents(userId);
        }

        public List<SpGetUserEventReturnModel> SmLoadUserEvent(SmEventItemIdDTO smEventData)
        {
            var loadEventData = new SmEventDataIdDTO
            {
                UserId = smEventData.UserId,
                EventDataId = smEventData.EventDataId
            };
            return _dashboardCommandsQueries.LoadEvent(loadEventData);
        }
    }
}
