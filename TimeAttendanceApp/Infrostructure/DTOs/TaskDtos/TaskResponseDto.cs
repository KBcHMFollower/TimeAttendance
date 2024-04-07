namespace TimeAttendanceApp.Infrostructure.DTOs.TaskDtos
{
    public class TaskResponseDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime cancelDate { get; set; }
    }
}
