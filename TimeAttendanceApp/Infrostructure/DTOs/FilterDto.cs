namespace TimeAttendanceApp.Infrostructure.DTOs
{
    public class FilterDto
    {
        public string OrderTarget { get; set; } = "Id";
        public string OrderType { get; set; } = "asc";
        public int page { get; set; } = 1;
        public int count { get; set; } = 10;
    }
}
