using SM.Database.DTOs;

namespace SM.Database.Commands.Dashboard
{
    public interface IDashboardCommands
    {
        public void SpSaveUserEvent(SmEventDataDTO smData);
        public void SpUpdateUserEvent(SmEventDataUpdateDTO smUpdateData);
        public List<SpDeleteUserEventReturnModel> SpDeleteUserEvent(SmEventDataIdDTO smData);
    }

    public class DashboardCommands : IDashboardCommands
    {
        private readonly MyDbContext _smContext;

        public DashboardCommands(MyDbContext context)
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
    }
}
