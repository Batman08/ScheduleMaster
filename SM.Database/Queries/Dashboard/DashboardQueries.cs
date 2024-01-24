using SM.Database.DTOs;

namespace SM.Database.Queries.Dashboard
{
    public interface IDashboardQueries
    {
        public List<SpGetUserEventsReturnModel> SpLoadUserEvents(string UserId);
        public List<SpGetUserEventReturnModel> SpLoadUserEvent(SmEventDataIdDTO smEventData);
    }

    public class DashboardQueries : IDashboardQueries
    {
        private readonly MyDbContext _smContext;

        public DashboardQueries(MyDbContext context)
        {
            _smContext = context;
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
