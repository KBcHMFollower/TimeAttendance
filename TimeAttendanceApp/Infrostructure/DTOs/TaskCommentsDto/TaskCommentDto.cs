namespace TimeAttendanceApp.Infrostructure.DTOs.TaskCommentsDto
{
    public class TaskCommentDto
    {
        public byte commentType {  get; set; }
        public IFormFile? file { get; set; }
        public string? text { get; set; }
        
    }
}
