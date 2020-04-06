using Microsoft.EntityFrameworkCore;

namespace ActivityCenter.Models
{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions options) : base(options){}
        public DbSet<User> Users { get; set;}
        public DbSet<Game> Games { get; set;}
        public DbSet<Association> Associations { get; set;}

    }
}