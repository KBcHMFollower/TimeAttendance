using System.ComponentModel.DataAnnotations;

namespace TimeAttendanceApp.Infrostructure.DTOs.TaskCommentsDto
{
    public class CommentRequestDto
    {
        [Required]
        public byte commentType {  get; set; }
        public IFormFile? file { get; set; }
        public string? text { get; set; }
        
    }
}
