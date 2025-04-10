using System.ComponentModel;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SeatType> SeatTypes { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookedSeat> BookedSeats { get; set; }
        public DbSet<PaymentSimulation> Payments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Vote> Votes { get; set; }


    }
}