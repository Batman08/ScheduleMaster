using SM.Database.DashboardCommandsQueries;

namespace SM.Database
{
    public interface ISmRepository
    {
        public void spSaveUserEvent(SmEventDataDTO smData);
        public List<SpGetUserEventsReturnModel> spLoadUserEvents(string UserId);
        public List<SpGetUserEventReturnModel> spLoadUserEvent(SmEventDataIdDTO smEventData);
    }

    public class SMRepository : ISmRepository
    {
        private readonly MyDbContext _smContext;

        public SMRepository(MyDbContext context)
        {
            _smContext = context;
        }

        public void spSaveUserEvent(SmEventDataDTO smData)
        {
            _smContext.SpSaveUserEvent(smData.UserId.ToString(), smData.Day, smData.Title, smData.Title, smData.Start, smData.End, smData.Colour);
        }

        public List<SpGetUserEventsReturnModel> spLoadUserEvents(string UserId)
        {
            return _smContext.SpGetUserEvents(UserId).ToList();
        }

        public List<SpGetUserEventReturnModel> spLoadUserEvent(SmEventDataIdDTO smEventData)
        {
            return _smContext.SpGetUserEvent(smEventData.EventDataId.ToString(), smEventData.UserId.ToString()).ToList();
        }
    }
}
