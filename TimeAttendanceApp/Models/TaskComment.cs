using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAttendanceApp.Models
{
    public class TaskComment
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }
        [ForeignKey("TaskId")]
        public Task Task { get; set; }
        [Required]
        public byte CommentType { get; set; }
        [Required]
        public byte[] Content { get; set; }
        public string? FileName { get; set; }
    }
}
