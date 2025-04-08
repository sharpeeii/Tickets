using System.ComponentModel;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<HallEntity> Halls { get; set; }
        public DbSet<SeatEntity> Seats { get; set; }
        public DbSet<SessionEntity> Sessions { get; set; }
        public DbSet<BookingEntity> Bookings { get; set; }
        public DbSet<BookedSeatEntity> BookedSeats { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<FilmEntity> Films { get; set; }
        public DbSet<VoteEntity> Votes { get; set; }
    }
}