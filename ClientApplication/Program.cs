using ClientApplication.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClientApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var jobClient = host.Services.GetRequiredService<JobClient>();
            var jobTypeClient = host.Services.GetRequiredService<JobTypeClient>();

            await ShowMenuAsync(jobClient, jobTypeClient);
        }

        private static IHostBuilder? CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string baseUrl = config["ServerSettings:BaseUrl"];

            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new Exception("BaseUrl could not be loaded from appsettings.json. Please verify that the file exists and has the correct settings.");
            }

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient<JobClient>(client => client.BaseAddress = new Uri(baseUrl))
                        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });

                    services.AddHttpClient<JobTypeClient>(client => client.BaseAddress = new Uri(baseUrl))
                        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                });
        }

        private static async Task ShowMenuAsync(JobClient jobClient, JobTypeClient jobTypeClient)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n=== Job Management CLI ===");
                Console.WriteLine("1. Create a new JobType");
                Console.WriteLine("2. List JobTypes");
                Console.WriteLine("3. Start a Job");
                Console.WriteLine("4. Get status of a Job");
                Console.WriteLine("5. List Running Jobs");
                Console.WriteLine("6. Cancel a Job");
                Console.WriteLine("7. Exit");
                Console.Write("Select an option: ");

                string? option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        await jobTypeClient.CreateJobTypeAsync();                        
                        break;
                    case "2":
                        await jobTypeClient.GetAllJobTypesAsync();
                        Console.WriteLine("\nPress any key to return to the main menu...");
                        Console.ReadKey();

                        break;
                    case "3":
                        await jobClient.StartJobAsync();
                        Console.WriteLine("\nPress any key to return to the main menu...");
                        Console.ReadKey();

                        break;
                    case "4":                        
                        await jobClient.GetJobStatusAsync();
                        Console.WriteLine("\nPress any key to return to the main menu...");
                        Console.ReadKey();

                        break;
                    case "5":
                        await jobClient.GetRunningJobsAsync();
                        Console.WriteLine("\nPress any key to return to the main menu...");
                        Console.ReadKey();
                        break;
                    case "6":
                        await jobClient.CancelJobAsync();
                        Console.WriteLine("\nPress any key to return to the main menu...");
                        Console.ReadKey();

                        break;
                    case "7":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid Option.");
                        Console.WriteLine("\nPress any key to return to the main menu...");
                        Console.ReadKey();

                        break;
                }
            }
        }
    }
}
