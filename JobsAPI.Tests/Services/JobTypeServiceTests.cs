using AutoMapper;
using JobsAPI.Automapper;
using JobsAPI.Data;
using JobsAPI.DTOs;
using JobsAPI.Models;
using JobsAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace JobsAPI.Tests.Services
{
    public class JobTypeServiceTests
    {
        private readonly JobTypeService _service;
        private readonly JobDbContext _context;
        private readonly IMapper _mapper;

        public JobTypeServiceTests()
        {
            var options = new DbContextOptionsBuilder<JobDbContext>()
             .UseInMemoryDatabase(databaseName: "TestDatabase")
             .Options;

            _context = new JobDbContext(options);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new JobProfile());
            }).CreateMapper();
            
            _mapper = mockMapper;

            _context = new JobDbContext(options);
            _service = new JobTypeService(_context, _mapper);
        }

        [Fact]
        public async Task GetAllJobTypesAsync_ReturnsJobTypes()
        {
            _context.JobTypes.Add(new JobType { JobTypeName = "Developer" });
            _context.JobTypes.Add(new JobType { JobTypeName = "Tester" });
            await _context.SaveChangesAsync();

            var result = await _service.GetAllJobTypesAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateJobTypeAsync_ReturnsFail_WhenDuplicate()
        {
            _context.JobTypes.Add(new JobType {  JobTypeName = "Developer" });
            await _context.SaveChangesAsync();

            var newJobType = new JobTypeDTO { JobTypeName = "Developer" };
            var result = await _service.CreateJobTypeAsync(newJobType);

            Assert.False(result.Success);
            Assert.Equal("JobType name already exists.", result.Message);
        }
    }
}
