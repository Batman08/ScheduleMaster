using Microsoft.EntityFrameworkCore;
using SM.Engine.Dashboard;

namespace SM.Database
{
    public interface ISmRepository
    {
        public SmEventDataDTO spSaveUserEvent(SmEventDataDTO smData);
    }

    public class SMRepository : ISmRepository
    {
        private readonly SMContext _smContext;

        public SMRepository(SMContext context)
        {
            _smContext = context;
        }

        public SmEventDataDTO spSaveUserEvent(SmEventDataDTO smData)
        {
            //SqlParameter p_UserId = new SqlParameter("@param", smData.UserId);
            //SqlParameter p_Title = new SqlParameter("@param", smData.Title);
            //SqlParameter p_Info = new SqlParameter("@param", smData.Info);
            //SqlParameter p_Start = new SqlParameter("@param", smData.Start);
            //SqlParameter p_End = new SqlParameter("@param", smData.End);
            //SqlParameter p_Colour = new SqlParameter("@param", smData.Colour);

            return _smContext.SaveEventData.FromSqlRaw("EXECUTE dbo.spGetMedicalPersons", smData).AsEnumerable().First();
        }
    }
}
