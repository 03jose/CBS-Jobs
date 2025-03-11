using System;

using JobsAPI.Models;
using JobsAPI.Utilities;

namespace JobsAPI.Tests.IService
{
    public interface IJobTypeService
    {
        Task<IEnumerable<JobType>> GetAllJobTypesAsync();
        Task<JobType?> GetJobTypeByIdAsync(int id);
        Task<ServiceResult> CreateJobTypeAsync(JobType jobType);
        Task<ServiceResult> UpdateJobTypeAsync(JobType jobType);
    }
}
