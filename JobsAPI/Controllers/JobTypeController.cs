using JobsAPI.DTOs;
using JobsAPI.Interfaces;
using JobsAPI.Models;
using JobsAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTypeController : ControllerBase
    {
        private readonly IJobTypeService _jobTypeService;

        public JobTypeController(IJobTypeService jobTypeService)
        {
            _jobTypeService = jobTypeService;
        }

        /// <summary>
        /// Get all job types.
        /// </summary>
        /// <returns>
        ///     200: Job types.
        ///     400: Error message.
        ///     404: Not found.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobTypeDTO>>> GetJobTypes()
        {
            return Ok(await _jobTypeService.GetAllJobTypesAsync());
        }

        /// <summary>
        /// Get a job type by its id.
        /// </summary>
        /// <param name="id">id of the job type</param>
        /// <returns>
        ///     200: Job type.
        ///     404: Not found.
        ///     400: Error message.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<JobType>> GetJobType(int id)
        {
            var jobType = await _jobTypeService.GetJobTypeByIdAsync(id);
            if (jobType == null) return NotFound();
            return Ok(jobType);
        }

        /// <summary>
        /// Create a new job type.
        /// </summary>
        /// <param name="jobType">object with the properties to create a job type</param>
        /// <returns>
        ///     200: Job type created.
        ///     400: Error message.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<JobType>> CreateJobType(JobTypeDTO jobType)
        {
            ServiceResult result = await _jobTypeService.CreateJobTypeAsync(jobType);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            //return CreatedAtAction(nameof(GetJobType), new { id = jobType.JobTypeId }, jobType);

            return Ok(result);
        }

        /// <summary>
        /// Update a job type.
        /// </summary>
        /// <param name="id">id of the job type to update </param>
        /// <param name="jobType">object with the modifications to do</param>
        /// <returns>
        ///     200: Job type updated.
        ///     400: Error message.
        ///     404: Not found.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobType(int id, JobTypeDTO jobType)
        {
            if (id != jobType.JobTypeId) return BadRequest();

            var result = await _jobTypeService.UpdateJobTypeAsync(jobType);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }
    }
}
