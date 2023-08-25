using DemoPractical.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoPractical.Data
{
    public class MovieDBContext: IdentityDbContext
    {
        public MovieDBContext(DbContextOptions<MovieDBContext> options): base(options)
        {
        }
        public MovieDBContext()
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieCast> MoviesCasts { get; set;}
    }
}
