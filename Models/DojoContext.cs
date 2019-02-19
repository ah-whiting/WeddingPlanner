using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Models
{
    public class DojoContext : DbContext
    {
        public DojoContext(DbContextOptions<DojoContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Wedding> weddings { get; set; }
        public DbSet<RSVP> rsvps { get; set; }
    }
}