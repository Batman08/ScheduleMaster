﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace SM.Database.DashboardCommandsQueries
{
    public class SmEventDataDTO
    {
        [Key]
        public Guid UserId { get; set; } = Guid.Empty;
        public string Title { get; set; } = "";
        public string Info { get; set; } = "";
        public string Start { get; set; } = "";
        public string End { get; set; } = "";
        public string Colour { get; set; } = "";
    }
}
