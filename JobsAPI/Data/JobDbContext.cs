using JobsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobsAPI.Data
{
    public class JobDbContext : DbContext
    {
        public JobDbContext(DbContextOptions<JobDbContext> options) : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; } = null!;
        public DbSet<JobType> JobTypes { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobType>()
                .HasMany(jt => jt.Jobs)
                .WithOne(j => j.JobType)
                .HasForeignKey(j => j.JobTypeId);
        }
    }
}

