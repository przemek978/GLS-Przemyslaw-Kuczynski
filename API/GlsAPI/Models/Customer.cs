using System.ComponentModel.DataAnnotations;

namespace GlsAPI.Models
{
    public class Customer
    {
        [Key] 
        public int Id { get; set; }
        [MaxLength(40)]
        public string Name1 { get; set; }
        [MaxLength(40)]
        public string? Name2 { get; set; }
        [MaxLength(40)]
        public string? Name3 { get; set; }
        [MaxLength(3)]
        public string Country { get; set; }
        [MaxLength(16)]
        public string ZipCode { get; set; }
        [MaxLength(30)]
        public string City { get; set; }
        [MaxLength(50)]
        public string Street { get; set; }
        [MaxLength(20)]
        public string? Phone { get; set; }
        [MaxLength(40)]
        public string? Contact { get; set; }

        public ICollection<Package> SentPackages { get; set; }
        public ICollection<Package> ReceivedPackages { get; set; }
    }
}
