using ImageUploaderWebMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageUploaderWebMVC.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Image> Images { get; set; }

        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                //Setup connection with database
                optionsBuilder.UseSqlServer("");
            }
        }*/

    }
}
