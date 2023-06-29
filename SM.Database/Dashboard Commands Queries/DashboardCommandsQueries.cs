//using SM.Engine.Dashboard;

namespace SM.Database.DashboardCommandsQueries
{
    public class DashboardCommandsQueries
    {
        private ISmRepository _smRepository;

        public DashboardCommandsQueries(ISmRepository smRepository)
        {
            _smRepository = smRepository;
        }

        public void SaveEvent(SmEventDataDTO smData)
        {
            _smRepository.SpSaveUserEvent(smData);
        }

        public void UpdateEvent(SmEventDataUpdateDTO smUpdateData)
        {
            _smRepository.SpUpdateUserEvent(smUpdateData);
        }

        public List<SpDeleteUserEventReturnModel> DeleteEvent(SmEventDataIdDTO smData)
        {
            return _smRepository.SpDeleteUserEvent(smData);
        }

        public List<SpGetUserEventsReturnModel> LoadEvents(Guid userId)
        {
            return _smRepository.SpLoadUserEvents(userId.ToString());
        }

        public List<SpGetUserEventReturnModel> LoadEvent(SmEventDataIdDTO smEventData)
        {
            return _smRepository.SpLoadUserEvent(smEventData);
        }
    }
}
