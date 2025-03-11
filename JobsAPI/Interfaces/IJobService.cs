using JobsAPI.DTOs;
using JobsAPI.Utilities;

namespace JobsAPI.Interfaces
{
    public interface IJobService
    {
        Task<ServiceResult> StartJobAsync(JobRequestDto request);
        Task<string> GetJobStatusAsync(int jobId);
        Task<bool> CancelJobAsync(int jobId);
        Task<List<JobDTO>> GetRunningJobsAsync();
    }
}
