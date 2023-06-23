using SM.Database.DashboardCommandsQueries;

namespace SM.Database
{
    public interface ISmRepository
    {
        public void spSaveUserEvent(SmEventDataDTO smData);
    }

    public class SMRepository : ISmRepository
    {
        private readonly SMContext _smContext;

        public SMRepository(SMContext context)
        {
            _smContext = context;
        }

        public void spSaveUserEvent(SmEventDataDTO smData)
        {
            var itemToAdd = new EventData
            {
                EventDataId = Guid.NewGuid().ToString(),
                UserId = smData.UserId.ToString(),
                Title = smData.Title,
                Info = smData.Info,
                Start = smData.Start,
                End = smData.End,
                Colour = smData.Colour
            };

            _smContext.SaveEventData.Add(itemToAdd);
            _smContext.SaveChanges();
            //_smContext.SaveEventData.FromSqlRaw($"EXECUTE dbo.spSaveUserEvent {smData.UserId}, {smData.Title}, {smData.Info}, {smData.Start}, {smData.UserId}").AsEnumerable().First();
        }
    }
}
