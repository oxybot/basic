namespace Basic.WebApi.DTOs
{
    public class UserCalendar
    {
        public UserCalendar()
        {
            this.Lines = new List<Line>();
        }

        public EntityReference User { get; set; }

        public ICollection<Line> Lines { get; set; }

        public class Line
        {
            public Line()
            {
                this.Days = new List<int>();
            }

            public string Category { get; set; }

            public ICollection<int> Days { get; set; }
        }
    }
}
