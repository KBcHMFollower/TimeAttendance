namespace TimeAttendanceApp.Infrostructure.DTOs.TaskCommentsDto
{
    public class CommentResponseDto
    {
        public string? text { get; set; }
        public byte commentType { get; set; }
        public byte[]? file { get; set; }
        public Guid id { get; set; }
    }
}
