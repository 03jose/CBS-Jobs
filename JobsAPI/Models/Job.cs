using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobsAPI.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; } 
        [Required]
        public int JobTypeId { get; set; }

        [ForeignKey("JobTypeId")]
        public JobType JobType { get; set; } = null!;
        
        [Required]
        public string JobName { get; set; } = string.Empty;
        public bool IsRunning { get; set; } = true;
        
    }

}
