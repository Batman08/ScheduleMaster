using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace SM.Database.DashboardCommandsQueries
{
    public class SmEventDataDTO
    {
        [Key]
        public Guid UserId { get; set; } = Guid.Empty;
        public string Day { get; set; } = "";
        public string Title { get; set; } = "";
        public string Info { get; set; } = "";
        public string Start { get; set; } = "";
        public string End { get; set; } = "";
        public string Colour { get; set; } = "";
    }

    public class SmEventDataIdDTO
    {
        [Key]
        public Guid EventDataId { get; set; } = Guid.Empty;
        [Key]
        public Guid UserId { get; set; } = Guid.Empty;
    }
}
