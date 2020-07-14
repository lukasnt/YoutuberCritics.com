using Microsoft.EntityFrameworkCore;
using YoutuberCritics.Models;

namespace YoutuberCritics.Data
{
    public class YoutuberCriticsContext : DbContext 
    {
        public YoutuberCriticsContext(DbContextOptions options) : base(options) { }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .Property(b => b.ChannelID)
                .HasDefaultValue(1);
            
            modelBuilder.Entity<Review>()
                .Property(b => b.UserID)
                .HasDefaultValue(1);

            modelBuilder.Entity<Channel>()
                .HasIndex(b => b.YoutubePath)
                .IsUnique();

            modelBuilder.Entity<Channel>()
                .Property(c => c.Subscribers)
                .HasDefaultValue(null);

            modelBuilder.Entity<Review>()
                .Property(r => r.DatePosted)
                .HasDefaultValueSql("getdate()");
        }

    }
}