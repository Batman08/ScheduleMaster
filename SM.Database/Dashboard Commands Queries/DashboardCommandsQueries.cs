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
            _smRepository.spSaveUserEvent(smData);
        }

        public List<SpGetUserEventsReturnModel> LoadEvents(Guid userId)
        {
            return _smRepository.spLoadUserEvents(userId.ToString());
        }

        public List<SpGetUserEventReturnModel> LoadEvent(SmEventDataIdDTO smEventData)
        {
            return _smRepository.spLoadUserEvent(smEventData);
        }
    }
}
