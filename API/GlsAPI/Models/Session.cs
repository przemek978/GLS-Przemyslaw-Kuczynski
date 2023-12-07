using System.ComponentModel.DataAnnotations;

namespace GlsAPI.Models
{
    public class Session
    {
        [Key]
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public DateTime ExpireDateTime { get; set; }
        public bool IsActive { get; set; } = true;
        public User User { get; set; }
    }
}
