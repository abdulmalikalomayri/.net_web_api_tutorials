using simpleapi.Models;

namespace simpleapi.Data
{
    public class DataContext : DbContext
    {
        /*
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        */
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Item> Items { get; set; } 
        
    }
}
