using JobsAPI.DTOs;
using JobsAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        /// <summary>
        /// start a new job in background.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        ///     200: Job started successfully.
        ///     400: Error message.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Start")]
        public async Task<IActionResult> StartJob([FromBody] JobRequestDto request)
        {
            try
            {
                var jobId = await _jobService.StartJobAsync(request);
                return Ok(new { JobId = jobId, Message = "Job started successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


        /// <summary>
        /// Get the status of a job by its id.
        /// </summary>
        /// <param name="jobId">Id to get the status</param>
        /// <returns>
        ///     200: Job status.
        ///     400: Error message.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("Status/{jobId}")]
        public async Task<IActionResult> GetJobStatus(int jobId)
        {
            try
            {
                var status = await _jobService.GetJobStatusAsync(jobId);
                return Ok(new { JobId = jobId, Status = status });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Cancel a job by its id.
        /// </summary>
        /// <param name="jobId">Job Id to cancel</param>
        /// <returns>
        ///     200. Job cancelled successfully.
        ///     404. Job not found or already completed.
        ///     400. Error message.
        ///     500. Internal server error.
        /// </returns>        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("cancel/{jobId}")]
        public async Task<IActionResult> CancelJob(int jobId)
        {
            var result = await _jobService.CancelJobAsync(jobId);
            if (!result)
                return NotFound(new { Message = "Job not found or already completed." });

            return Ok(new { JobId = jobId, Message = "Job cancelled successfully." });
        }

        /// <summary>
        /// Get jobs in running status
        /// </summary>
        /// <returns>
        ///     200. List of running jobs.
        ///     
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        [HttpGet("running")]
        public async Task<IActionResult> GetRunningJobs()
        {
            var jobs = await _jobService.GetRunningJobsAsync();
            return Ok(jobs);
        }
    }
}
