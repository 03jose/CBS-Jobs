using AutoMapper;
using JobsAPI.Data;
using JobsAPI.DTOs;
using JobsAPI.Interfaces;
using JobsAPI.Models;
using JobsAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System;

namespace JobsAPI.Services
{
    public class JobTypeService : IJobTypeService
    {
        private readonly JobDbContext _context;
        private readonly IMapper _mapper;

        public JobTypeService(JobDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobTypeDTO>> GetAllJobTypesAsync()
        {
            return await  _context.JobTypes
                .Select(jt => _mapper.Map<JobTypeDTO>(jt)).ToListAsync(); 
        }

        public async Task<JobTypeDTO?> GetJobTypeByIdAsync(int id)
        {
            return await _context.JobTypes
                .Where(jt => jt.JobTypeId == id)
                .Select(jt => _mapper.Map<JobTypeDTO>(jt))
                .FirstOrDefaultAsync();
        }

        public async Task<ServiceResult> CreateJobTypeAsync(JobTypeDTO jobType)
        {
            if (await _context.JobTypes.AnyAsync(jt => jt.JobTypeName == jobType.JobTypeName))
            {
                //return new ServiceResult { Success = false, Message = "JobType name already exists." };
                return ServiceResult.Fail("JobType name already exists.");
            }

            JobType newJobType =  _mapper.Map<JobType>(jobType);

            _context.JobTypes.Add(newJobType);
            await _context.SaveChangesAsync();
            return new ServiceResult { Success = true };
        }

        public async Task<ServiceResult> UpdateJobTypeAsync(JobTypeDTO jobType)
        {
            if (await _context.JobTypes.AnyAsync(jt => jt.JobTypeName == jobType.JobTypeName && jt.JobTypeId != jobType.JobTypeId))
            {
                return new ServiceResult { Success = false, Message = "Another JobType with the same name exists." };
            }

            JobType newJobType = _mapper.Map<JobType>(jobType);

            _context.Entry(newJobType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return new ServiceResult { Success = true };
        }
    }
}
