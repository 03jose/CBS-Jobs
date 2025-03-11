using JobsAPI.Controllers;
using JobsAPI.Models;
using JobsAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using JobsAPI.Interfaces;
using JobsAPI.Utilities;
using JobsAPI.DTOs;

namespace JobsAPI.Tests.Controllers
{
    public class JobTypeControllerTests
    {
        private readonly Mock<IJobTypeService> _mockService;
        private readonly JobTypeController _controller;

        public JobTypeControllerTests()
        {
            _mockService = new Mock<IJobTypeService>();
            _controller = new JobTypeController(_mockService.Object);
        }

        [Fact]
        public async Task GetJobTypes_ReturnsOk_WithJobTypes()
        {
            // Arrange
            var jobTypes = new List<JobTypeDTO>
            {
                new JobTypeDTO { JobTypeId= 1, JobTypeName = "Developer" },
                new JobTypeDTO { JobTypeId = 2, JobTypeName = "Tester" }
            };

            _mockService.Setup(service => service.GetAllJobTypesAsync())
                        .ReturnsAsync(jobTypes);

            // Act
            var result = await _controller.GetJobTypes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnJobTypes = Assert.IsType<List<JobTypeDTO>>(okResult.Value);
            Assert.Equal(2, returnJobTypes.Count);
        }

        [Fact]
        public async Task CreateJobType_ReturnsBadRequest_WhenNameExists()
        {
            // Arrange
            var jobType = new JobTypeDTO { JobTypeName = "Developer" };

            _mockService.Setup(service => service.CreateJobTypeAsync(jobType))
                        .ReturnsAsync(ServiceResult.Fail("JobType already exists."));

            // Act
            var result = await _controller.CreateJobType(jobType);

            // Assert
            //var badRequestResult = Assert.IsType<ActionResult<JobTypeDTO>>(result);
            //var objectResult = Assert.IsType<BadRequestObjectResult>(badRequestResult.Result);
            //Assert.Equal("JobType already exists.", objectResult.Value);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("JobType already exists.", badRequestResult.Value);
        }
    }
}
