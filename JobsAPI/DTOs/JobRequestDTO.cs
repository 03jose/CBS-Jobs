namespace JobsAPI.DTOs
{
    public class JobRequestDto
    {
        public string JobTypeName { get; set; } = string.Empty;
        public string JobName { get; set; } = string.Empty;

        public int JobTypeId { get; set; }
    }
}
