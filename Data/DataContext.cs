namespace simpleapi.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlServer("Data Source=MALEK;Initial Catalog=apicrud;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        // if we run migrations it will take below classes and make them as DB tables 
        public DbSet<HotelBooking> Bookings { get; set; }
        public DbSet<User> Users => Set<User>();
    }
}
