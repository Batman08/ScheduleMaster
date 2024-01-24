using SM.Database;
using SM.Database.Commands.Dashboard;
using SM.Database.DashboardCommandsQueries;
using SM.Database.Queries.Dashboard;

namespace SM.Engine.DashboardModule
{
    public class HomeModule
    {
        private IDashboardCommands _dashboardCommands;
        private IDashboardQueries _dashboardQueries;

        public HomeModule(IDashboardCommands dashboardCommands, IDashboardQueries dashboardQueries)
        {
            _dashboardCommands = dashboardCommands;
            _dashboardQueries = dashboardQueries;
        }

        private bool SmCheckDataIsValid<T>(T data)
        {
            var properties = data!.GetType().GetProperties();
            foreach (var property in properties)
            {
                bool isInfoProperty = property.Name == "Info";
                if (isInfoProperty) continue;

                bool isPropertyValueValid = property.GetValue(data) == null || string.IsNullOrEmpty(property.GetValue(data)!.ToString()) || string.IsNullOrWhiteSpace(property.GetValue(data)!.ToString());
                if (isPropertyValueValid) return false;
            }

            return true;
        }

        public SmEventFormResponseDTO SmSaveUserEvent(SmEventItemDTO smData)
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

            //create object SmEventFormResponseDTO copying values of SmEventItemDTO
            var smEventFormResponseDTO = new SmEventFormResponseDTO
            {
                SmEventItem = new SmEventItemResponseDTO
                {
                    EventDataId = smData.EventDataId,
                    Day = smData.Day,
                    Title = smData.Title,
                    Info = smData.Info,
                    Start = smData.Start,
                    End = smData.End,
                    Colour = smData.Colour
                }
            };

            if (SmCheckDataIsValid(smData))
                _dashboardCommands.SpSaveUserEvent(dataToSave);
            else
                smEventFormResponseDTO.Message = "An error occured. One or more required fields were not filled out!";

            return smEventFormResponseDTO;
        }

        public SmEventFormResponseDTO SmUpdateUserEvent(SmEventItemUpdateDTO smUpdateData)
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

            //create object SmEventFormResponseDTO copying values of SmEventItemDTO
            var smEventFormResponseDTO = new SmEventFormResponseDTO
            {
                SmEventItem = new SmEventItemResponseDTO
                {
                    EventDataId = smUpdateData.EventDataId,
                    Day = smUpdateData.Day,
                    Title = smUpdateData.Title,
                    Info = smUpdateData.Info,
                    Start = smUpdateData.Start,
                    End = smUpdateData.End,
                    Colour = smUpdateData.Colour
                }
            };

            if (SmCheckDataIsValid(smUpdateData))
                _dashboardCommands.SpUpdateUserEvent(dataToSave);
            else
                smEventFormResponseDTO.Message = "An error occured. One or more required fields were not filled out!";

            return smEventFormResponseDTO;
        }

        public string SmDeleteUserEvent(SmEventItemIdDTO smData)
        {
            var dataToSave = new SmEventDataIdDTO
            {
                UserId = smData.UserId,
                EventDataId = smData.EventDataId,
            };
            return _dashboardCommands.SpDeleteUserEvent(dataToSave).First().Message;
        }

        public List<SpGetUserEventsReturnModel> SmLoadUserEvents(Guid userId)
        {
            return _dashboardQueries.SpLoadUserEvents(userId.ToString());
        }

        public List<SpGetUserEventReturnModel> SmLoadUserEvent(SmEventItemIdDTO smEventData)
        {
            var loadEventData = new SmEventDataIdDTO
            {
                UserId = smEventData.UserId,
                EventDataId = smEventData.EventDataId
            };
            return _dashboardQueries.SpLoadUserEvent(loadEventData);
        }
    }
}
