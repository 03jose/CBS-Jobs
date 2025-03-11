using AutoMapper;
using Castle.Core.Logging;
using JobsAPI.Data;
using JobsAPI.DTOs;
using JobsAPI.Models;
using JobsAPI.Repositories.Interface;
using JobsAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JobsAPI.Tests.Services
{
    public class JobServiceTests
    {
        private readonly Mock<IJobRepository> _mockJobRepository;
        private readonly Mock<ILogger<JobService>> _mockLogger;
        private readonly JobService _service;
        
        public JobServiceTests()
        {
            _mockJobRepository = new Mock<IJobRepository>();
            _mockLogger = new Mock<ILogger<JobService>>();
            _service = new JobService(_mockJobRepository.Object, _mockLogger.Object);
        }

        [Fact]        
        public async Task StartJobAsync_ReturnsSuccess_WhenValid()
        {
            // Arrange
            var jobDto = new JobRequestDto { JobTypeId = 1, JobName = "NewJob", JobTypeName = "JobType1" }; 
            var jobDb = new Job { JobId = 1, JobTypeId = 1, JobName = "NewJob" };

            // Mock de GetRunningJobsByTypeAsync
            _mockJobRepository.Setup(repo => repo.GetRunningJobsByTypeAsync(It.IsAny<int>()))
                              .ReturnsAsync(new List<Job>()); 

            // Mock de AddJobAsync
            _mockJobRepository.Setup(repo => repo.AddJobAsync(It.IsAny<Job>()))
                              .ReturnsAsync(jobDb);

            Assert.NotNull(_mockJobRepository.Object);

            // Act
            var result = await _service.StartJobAsync(jobDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

    }
}