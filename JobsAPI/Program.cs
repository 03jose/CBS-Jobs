using JobsAPI.Data;
using JobsAPI.Interfaces;
using JobsAPI.Repositories.Interface;
using JobsAPI.Repositories.Services;
using JobsAPI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// connection to MySQL
builder.Services.AddDbContext<JobDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Does not found the connection String.");
}

// Check connection to MySQL
using var connection = new MySqlConnector.MySqlConnection(connectionString);
try
{
    await connection.OpenAsync();
    Console.WriteLine("Connection to MySQL success.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error to connect to MySQL: {ex.Message}");
}


// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/server.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IJobTypeService, JobTypeService>();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// test the application
try
{
    Log.Information("Starting application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

app.MapGet("/", () => "Job Management API is running!");

app.Run();
