
using JobsAPI.Models;


namespace JobsAPI.Repositories.Interface
{
    public interface IJobRepository
    {
        Task<Job> AddJobAsync(Job job);
        Task<Job?> GetJobByIdAsync(int jobId);
        Task<List<Job>> GetRunningJobsByTypeAsync(int jobTypeId);
        Task<List<Job>> GetAllRunningJobsAsync();
        Task<bool> UpdateJobAsync(Job job);
        Task<bool> CancelJobAsync(int jobId);
        Task<JobType?> GetJobTypeByNameAsync(string jobTypeName);
    }
}
