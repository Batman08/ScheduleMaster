namespace SM.Engine.DashboardModule
{
    public class SmEventItemDTO
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public Guid EventDataId { get; set; } = Guid.NewGuid();
        public string Day { get; set; } = "";
        public string Title { get; set; } = "";
        public string Info { get; set; } = "";
        public string Start { get; set; } = "";
        public string End { get; set; } = "";
        public string Colour { get; set; } = "";
    }

    public class SmEventItemIdDTO
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public Guid EventDataId { get; set; } = Guid.Empty;
    }

    public class SmEventItemUpdateDTO
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public Guid EventDataId { get; set; } = Guid.Empty;
        public string Day { get; set; } = "";
        public string Title { get; set; } = "";
        public string Info { get; set; } = "";
        public string Start { get; set; } = "";
        public string End { get; set; } = "";
        public string Colour { get; set; } = "";
    }

    public class SmMessageItemDTO
    {
        public string Message { get; set; } = "";
    }
}
