using Microsoft.EntityFrameworkCore;
using User.Models;

namespace User.Data
{
    public class WorkDbContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public WorkDbContext(DbContextOptions<WorkDbContext> options): base(options)
        {
            
        }

    }
}
