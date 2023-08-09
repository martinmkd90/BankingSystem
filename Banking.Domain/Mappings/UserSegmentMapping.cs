using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banking.Domain.Models;

namespace Banking.Domain.Mappings
{
    public class UserSegmentMapping
    {
        public int UserSegmentMappingID { get; set; } // This is the primary key
        public int UserID { get; set; }
        public User User { get; set; } = new User(); // Navigation property
        public int SegmentID { get; set; }
        public Segment Segment { get; set; } = new Segment(); // Navigation property
    }
}
