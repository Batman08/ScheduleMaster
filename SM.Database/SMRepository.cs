using SM.Database.DashboardCommandsQueries;

namespace SM.Database
{
    public interface ISmRepository
    {
        public void spSaveUserEvent(SmEventDataDTO smData);
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
            //var itemToAdd = new EventData
            //{
            //    EventDataId = Guid.NewGuid().ToString(),
            //    UserId = smData.UserId.ToString(),
            //    Title = smData.Title,
            //    Info = smData.Info,
            //    Start = smData.Start,
            //    End = smData.End,
            //    Colour = smData.Colour
            //};
            //_smContext.SaveChanges();

            _smContext.SpSaveUserEvent(smData.UserId.ToString(), smData.Title, smData.Title, smData.Start, smData.End, smData.Colour);
        }
    }
}
