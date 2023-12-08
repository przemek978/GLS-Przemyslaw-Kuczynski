using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlsAPI.Models
{
    public class Package
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [MaxLength(25)]
        public string References { get; set; }
        [MaxLength(80)]
        public string Notes { get; set; }
        public int Quantity { get; set; }
        public float Weight { get; set; }
        public DateTime Date {  get; set; }
        public int SenderId { get; set; }
        public Customer Sender { get; set; }
        public int RecipientId { get; set; }
        public Customer Recipient { get; set; }
        public int StatusId { get; set; }
        public PackageStatus Status { get; set;}
    }
}
