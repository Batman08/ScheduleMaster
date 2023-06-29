using SM.Database.DashboardCommandsQueries;

namespace SM.Database
{
    public interface ISmRepository
    {
        public void SpSaveUserEvent(SmEventDataDTO smData);
        public void SpUpdateUserEvent(SmEventDataUpdateDTO smUpdateData);
        public List<SpDeleteUserEventReturnModel> SpDeleteUserEvent(SmEventDataIdDTO smData);
        public List<SpGetUserEventsReturnModel> SpLoadUserEvents(string UserId);
        public List<SpGetUserEventReturnModel> SpLoadUserEvent(SmEventDataIdDTO smEventData);
    }

    public class SMRepository : ISmRepository
    {
        private readonly MyDbContext _smContext;

        public SMRepository(MyDbContext context)
        {
            _smContext = context;
        }

        public void SpSaveUserEvent(SmEventDataDTO smData)
        {
            _smContext.SpSaveUserEvent(smData.EventDataId.ToString(), smData.UserId.ToString(), smData.Day, smData.Title, smData.Info, smData.Start, smData.End, smData.Colour);
        }

        public void SpUpdateUserEvent(SmEventDataUpdateDTO smUpdateData)
        {
            _smContext.SpUpdateUserEvent(smUpdateData.EventDataId.ToString(), smUpdateData.UserId.ToString(), smUpdateData.Day, smUpdateData.Title, smUpdateData.Info, smUpdateData.Start, smUpdateData.End, smUpdateData.Colour);
        }

        public List<SpDeleteUserEventReturnModel> SpDeleteUserEvent(SmEventDataIdDTO smData)
        {
            return _smContext.SpDeleteUserEvent(smData.EventDataId.ToString(), smData.UserId.ToString());
        }

        public List<SpGetUserEventsReturnModel> SpLoadUserEvents(string UserId)
        {
            return _smContext.SpGetUserEvents(UserId).ToList();
        }

        public List<SpGetUserEventReturnModel> SpLoadUserEvent(SmEventDataIdDTO smEventData)
        {
            return _smContext.SpGetUserEvent(smEventData.EventDataId.ToString(), smEventData.UserId.ToString()).ToList();
        }
    }
}
