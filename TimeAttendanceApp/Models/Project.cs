using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAttendanceApp.Models
{
    public class Project
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(255)")]
        public string ProjectName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
