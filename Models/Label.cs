using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triggers.Models
{
    public class Label
    {
        public int Id { get; set; }
        public int PackageId { get; set; }  
        public string LabelFile { get; set; }
        public bool IsPrinted { get; set; } = false;
    }
}
