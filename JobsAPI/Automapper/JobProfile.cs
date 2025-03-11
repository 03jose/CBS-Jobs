using AutoMapper;
using JobsAPI.DTOs;
using JobsAPI.Models;

namespace JobsAPI.Automapper
{
    public class JobProfile : Profile
    {
        public JobProfile() { 
            CreateMap<Job, JobRequestDto>().ReverseMap();
            CreateMap<Job, JobDTO>().ReverseMap();

            CreateMap<JobType, JobTypeDTO>().ReverseMap();
        }

    }
}
