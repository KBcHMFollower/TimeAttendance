using System.ComponentModel.DataAnnotations;

namespace TimeAttendanceApp.Infrostructure.DTOs.TaskDto
{
    public class TaskRequestDto
    {
        [Required]
        public string name { get; set; }
        public Guid? prijectId { get; set; }
        [Required]
        public DateTime startDate { get; set; } = DateTime.Now;
    }
}
