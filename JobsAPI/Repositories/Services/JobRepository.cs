using JobsAPI.Data;
using JobsAPI.DTOs;
using JobsAPI.Models;
using JobsAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace JobsAPI.Repositories.Services
{
    // Access the database to perform CRUD operations on Job entities
    public class JobRepository : IJobRepository
    {
        private readonly JobDbContext _context;

        public JobRepository(JobDbContext context)
        {
            _context = context;
        }

        public async Task<Job> AddJobAsync(Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return job;
        }

        public async Task<Job?> GetJobByIdAsync(int jobId)
        {
            return await _context.Jobs.FindAsync(jobId);
        }

        public async Task<List<Job>> GetRunningJobsByTypeAsync(int jobTypeId)
        {
            return await _context.Jobs
                .Where(j => j.JobTypeId == jobTypeId && j.IsRunning)
                .ToListAsync();
        }

        public async Task<List<Job>> GetAllRunningJobsAsync()
        {
            await _context.JobTypes.LoadAsync();
            return await _context.Jobs
                .Where(j => j.IsRunning)
                .ToListAsync();
        }

        public async Task<JobType?> GetJobTypeByNameAsync(string jobTypeName)
        {
            return await _context.JobTypes
                .FirstOrDefaultAsync(jt => jt.JobTypeName == jobTypeName);  
        }


        public async Task<bool> UpdateJobAsync(Job job)
        {
            _context.Jobs.Update(job);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CancelJobAsync(int jobId)
        {
            var job = await GetJobByIdAsync(jobId);
            if (job == null) return false;

            job.IsRunning = false;
            _context.Jobs.Update(job);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
