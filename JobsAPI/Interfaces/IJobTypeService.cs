using JobsAPI.DTOs;
using JobsAPI.Models;
using JobsAPI.Utilities;

namespace JobsAPI.Interfaces
{
    public interface IJobTypeService
    {
        Task<IEnumerable<JobTypeDTO>> GetAllJobTypesAsync();
        Task<JobTypeDTO?> GetJobTypeByIdAsync(int id);
        Task<ServiceResult> CreateJobTypeAsync(JobTypeDTO jobType);
        Task<ServiceResult> UpdateJobTypeAsync(JobTypeDTO jobType);
    }
}
