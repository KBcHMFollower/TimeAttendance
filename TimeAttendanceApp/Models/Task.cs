using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TimeAttendanceApp.Models
{
    public class Task
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(255)")]
        public string TaskName { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime CancelDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<TaskComment> Comments { get; set; } = new List<TaskComment>();
    }
}
