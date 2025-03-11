using JobsAPI.DTOs;
using JobsAPI.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientApplication.Services
{
    public class JobClient
    {
        private readonly HttpClient _httpClient;

        public JobClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task StartJobAsync()
        {
            try
            {
                Console.Clear();

                Console.WriteLine("Fetching JobTypes...");
                HttpResponseMessage responseJobTypes = await _httpClient.GetAsync($"api/JobType");

                if (!responseJobTypes.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error fetching JobTypes: {responseJobTypes.StatusCode}");
                    Console.WriteLine("\nPress any key to return to the main menu...");
                    Console.ReadKey();
                    return;
                }

                string jsonResponse = await responseJobTypes.Content.ReadAsStringAsync();
                var jobTypes = JsonSerializer.Deserialize<List<JobTypeDTO>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (jobTypes == null || jobTypes.Count == 0)
                {
                    Console.WriteLine("No running jobs types found.");
                    return;
                }

                Console.WriteLine("Select a JobType:");
                for (int i = 0; i < jobTypes.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {jobTypes[i].JobTypeName} (ID: {jobTypes[i].JobTypeId})");
                }

                Console.Write("Enter the number of the JobType: ");
                if (!int.TryParse(Console.ReadLine(), out int selectedIndex) || selectedIndex < 1 || selectedIndex > jobTypes.Count)
                {
                    Console.WriteLine("Invalid selection.");
                    Console.WriteLine("\nPress any key to return to the main menu...");
                    Console.ReadKey();
                    return;
                }

                int jobTypeId = jobTypes[selectedIndex - 1].JobTypeId;

                Console.Write("Enter the Job Name: ");
                string jobName = Console.ReadLine() ?? "DefaultJob";

                var job = new { JobTypeId = jobTypeId, JobName = jobName  };
                var jsonContent = new StringContent(JsonSerializer.Serialize(job), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Jobs/Start", jsonContent);
                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Job started successfully: {result}");
                }
                else
                {
                    Console.WriteLine($"Failed to start job: {result}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting job: {ex.Message}");
            }
        }

        public async Task GetJobStatusAsync()
        {
            try
            {
                Console.Write("Enter Job ID: ");
                string jobId = Console.ReadLine() ?? "";

                var response = await _httpClient.GetAsync($"api/jobs/status/{jobId}");

                if (response.IsSuccessStatusCode)
                {
                    var status = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Job Status: {status}");
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve job status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving job status: {ex.Message}");
            }
        }

        public async Task CancelJobAsync()
        {
            try
            {
                Console.Write("Enter Job ID to cancel: ");
                string jobId = Console.ReadLine() ?? "";

                if (string.IsNullOrEmpty(jobId))
                {
                    Console.WriteLine("The id is empty.");
                    return;
                }

                var response = await _httpClient.PostAsync($"api/jobs/cancel/{jobId}", null);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Job canceled successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to cancel job: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error canceling job: {ex.Message}");
            }
        }

        public async Task GetRunningJobsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/jobs/running");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var jobs = JsonSerializer.Deserialize<List<JobDTO>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (jobs == null || jobs.Count == 0)
                    {
                        Console.WriteLine("No running jobs found.");
                        return;
                    }

                    Console.WriteLine("\nRunning Jobs:");
                    for (int i = 0; i < jobs.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {jobs[i].JobTypeName} (ID: {jobs[i].JobId})");
                    }

                    int counterJobs = jobs.Count;

                    while (true)
                    {
                        Console.Write("\nDo you want to cancel any job? (y/n): ");
                        string? cancelOption = Console.ReadLine()?.ToLower();

                        if (cancelOption == "y")
                        {
                            Console.Write("Enter the job number to cancel: ");
                            if (int.TryParse(Console.ReadLine(), out int jobIndex) && jobIndex > 0 && jobIndex <= jobs.Count)
                            {
                                string jobId = jobs[jobIndex - 1].JobId.ToString();
                                var cancelResponse = await _httpClient.PostAsync($"api/jobs/cancel/{jobId}", null);

                                if (cancelResponse.IsSuccessStatusCode)
                                {
                                    Console.WriteLine("Job canceled successfully.");
                                    counterJobs--;
                                    if (counterJobs == 0) break;

                                }
                                else
                                {
                                    Console.WriteLine($"Failed to cancel job: {cancelResponse.StatusCode}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid job number. Try again.");
                            }
                        }
                        else if (cancelOption == "n")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid option. Please enter 'y' or 'n'.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve running jobs: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving running jobs: {ex.Message}");
            }
        }

    }
}
