using System.ComponentModel.DataAnnotations;

namespace TimeAttendanceApp.Infrostructure.DTOs.ProjectDtos
{
    public class ProjectRequestDto
    {
        [Required]
        public string name { get; set; }
    }
}
