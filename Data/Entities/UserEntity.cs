using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(100)]
        public string PasswordHash { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Role { get; set; } = "User";
        public virtual ICollection<ReservationEntity>? Reservations { get; set; }
        public virtual ICollection<VoteEntity>? Votes { get; set; }
    }
}