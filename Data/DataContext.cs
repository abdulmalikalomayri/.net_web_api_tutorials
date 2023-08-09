using Microsoft.EntityFrameworkCore;
using simpleapi.Model;

namespace simpleapi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        // add table using model 
        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }

    }
}
