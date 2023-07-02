using Microsoft.EntityFrameworkCore;
using simpleapi.Models;

namespace simpleapi.Data
{
    public class ApiContext : DbContext
    {

        public DbSet<HotelBooking> Bookings { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }
    }
}
