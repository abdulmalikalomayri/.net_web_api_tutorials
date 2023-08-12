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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        
        // join table can't be abstracted 
        // we should do OnModelCreateing 
        // linking the relashin
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonCategory>().HasKey(pc => new { pc.PokemonId, pc.CategoryId });
            modelBuilder.Entity<PokemonCategory>().HasOne(p => p.Pokemon).WithMany(pc => pc.PokemonCategories).HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonCategory>().HasOne(p => p.Pokemon).WithMany(pc => pc.PokemonCategories).HasForeignKey(c => c.CategoryId);


            modelBuilder.Entity<PokemonOwner>().HasKey(po => new { po.PokemonId, po.OwnerId });
            modelBuilder.Entity<PokemonOwner>().HasOne(p => p.Pokemon).WithMany(po => po.PokemonOwners).HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonOwner>().HasOne(p => p.Pokemon).WithMany(po => po.PokemonOwners).HasForeignKey(c => c.OwnerId);
        }

<<<<<<< HEAD
=======
        // if we run migrations it will take below classes and make them as DB tables 
        public DbSet<HotelBooking> Bookings { get; set; }
        public DbSet<User> Users => Set<User>();
>>>>>>> f6f6601b347329a744228e8c0fc7a69f5cd153e2
    }
}
