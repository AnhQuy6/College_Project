using CollegeApp.Data.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CollegeApp.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options) 
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentConfig());
            modelBuilder.ApplyConfiguration(new StudentConfig());
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        
    }
}
