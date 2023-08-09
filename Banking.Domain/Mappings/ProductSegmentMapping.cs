using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banking.Domain.Models;

namespace Banking.Domain.Mappings
{
    public class ProductSegmentMapping
    {
        public int ProductID { get; set; }
        public Product Product { get; set; } = new Product(); // Navigation property
        public int SegmentID { get; set; }
        public Segment Segment { get; set; } = new Segment(); // Navigation property
    }
}
