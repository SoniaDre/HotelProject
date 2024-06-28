using HotelSo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelSo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<AmenitiesPerRoom> Amenities { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                       .HasOne(i => i.Room)
                       .WithMany(a => a.Reservations)
                       .HasForeignKey(i => i.RoomId)
                       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
                     .HasOne(i => i.Amenities)
                     .WithMany(a => a.Rooms)
                     .HasForeignKey(i => i.AmenitiesId)
                     .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
