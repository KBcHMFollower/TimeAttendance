namespace TimeAttendanceApp.Infrostructure.DTOs.TaskDtos
{
    public class TaskUpdateDto
    {
        public string? name { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
    }
}
