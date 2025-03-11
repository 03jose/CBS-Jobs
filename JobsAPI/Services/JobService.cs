using AutoMapper;
using JobsAPI.DTOs;
using JobsAPI.Interfaces;
using JobsAPI.Models;
using JobsAPI.Repositories.Interface;
using JobsAPI.Utilities;

namespace JobsAPI.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly ILogger<JobService> _logger;
        

        public JobService(IJobRepository jobRepository, ILogger<JobService> logger)
        {
            _logger = logger;
            
            _jobRepository = jobRepository;
        }

        public async Task<ServiceResult> StartJobAsync(JobRequestDto request)
        {
            var runningJobs = await _jobRepository.GetRunningJobsByTypeAsync(request.JobTypeId);
            if (runningJobs.Count >= 5)
            {
                _logger.LogWarning("Limit reached for {JobType}. No more jobs can be started.", request.JobTypeName);
                throw new InvalidOperationException($"No more than 5 jobs of the type can be executed. {request.JobTypeName}.");
            }

            var job = new Job { JobTypeId = request.JobTypeId, JobName = request.JobName };
            await _jobRepository.AddJobAsync(job);
            _logger.LogInformation("Job started with ID {JobId}", job.JobId);

            return ServiceResult.Ok();
        }

        public async Task<string> GetJobStatusAsync(int jobId)
        {
            var job = await _jobRepository.GetJobByIdAsync(jobId);
            return job?.IsRunning == true ? "Running" : "Not Running";
        }

        public async Task<bool> CancelJobAsync(int jobId)
        {
            return await _jobRepository.CancelJobAsync(jobId);
        }


        public async Task<List<JobDTO>> GetRunningJobsAsync()
        {
            var jobs = await _jobRepository.GetAllRunningJobsAsync(); 

            return jobs.Select(job => new JobDTO 
            { 
                JobId = job.JobId, 
                JobTypeName = job.JobType.JobTypeName, 
                JobName = job.JobName
            }).ToList();
        }
    }
}
