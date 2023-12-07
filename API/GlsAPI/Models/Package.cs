using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlsAPI.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string PackageNumber { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
