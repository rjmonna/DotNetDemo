using DotNetDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetDemo.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Department>()
                .HasKey(p => p.DepartmentId);

            modelBuilder
                .Entity<Employee>()
                .HasKey(p => p.EmployeeId);

            modelBuilder
                .Entity<Department>()
                .HasMany(e => e.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId);
        }
    }
}