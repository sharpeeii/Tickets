using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        [MaxLength(10)]
        public string Role { get; set; } = "User";
        public virtual ICollection<Booking>? Reservations { get; set; }
        public virtual ICollection<Vote>? Votes { get; set; }
    }
}