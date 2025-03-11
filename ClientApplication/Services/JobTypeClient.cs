using JobsAPI.DTOs;
using System.Text;
using System.Text.Json;

namespace ClientApplication.Services
{
    public class JobTypeClient
    {
        private readonly HttpClient _httpClient;

        public JobTypeClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task GetAllJobTypesAsync()
        {
            Console.Clear();
            var response = await _httpClient.GetAsync("api/JobType");
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching JobTypes: {response.StatusCode}");
                
                return;
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(jsonResponse))
            {
                Console.WriteLine("No JobTypes found.");
                
                return;
            }

            var jobTypes = JsonSerializer.Deserialize<List<JobTypeDTO>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (jobTypes == null || jobTypes.Count == 0)
            {
                Console.WriteLine("No JobTypes found.");
            }
            else
            {
                Console.WriteLine("JobTypes List:");
                foreach (var jobType in jobTypes)
                {
                    Console.WriteLine($"- ID: {jobType.JobTypeId}, Name: {jobType.JobTypeName}");
                }
            }

        }

        public async Task CreateJobTypeAsync()
        {
            Console.Clear();
            Console.Write("Insert the JobType Name: ");
            string name = Console.ReadLine() ?? "DefaultJobType";

            var jobType = new { JobTypeName = name };
            var jsonContent = new StringContent(JsonSerializer.Serialize(jobType), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/jobtype", jsonContent);

            string result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"JobType created successfully: {result}");
            }
            else
            {
                Console.WriteLine($"Error creating JobType: {response.StatusCode} - {result}");
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }

    }
}
