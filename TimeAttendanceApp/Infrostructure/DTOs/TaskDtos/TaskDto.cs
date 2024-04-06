namespace TimeAttendanceApp.Infrostructure.DTOs.TaskDto
{
    public class TaskDto
    {
        public string taskName { get; set; }
        public Guid? prijectId { get; set; }
        public DateTime startDate { get; set; } = DateTime.Now;
    }
}
