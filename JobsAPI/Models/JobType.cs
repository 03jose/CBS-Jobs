using System.ComponentModel.DataAnnotations;

namespace JobsAPI.Models
{
    public class JobType
    {
        [Key]
        public int JobTypeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string JobTypeName { get; set; }
        public string JobDescription { get; set; } = string.Empty;


        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
