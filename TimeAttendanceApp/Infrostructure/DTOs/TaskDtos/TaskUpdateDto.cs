namespace TimeAttendanceApp.Infrostructure.DTOs.TaskDtos
{
    public class TaskUpdateDto
    {
        public string? taskName { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
    }
}
