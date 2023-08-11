using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Features { get; set; } // This can be a JSON string
        public string Benefits { get; set; }
        public string TermsAndConditions { get; set; }
        public string EligibilityCriteria { get; set; }
        public string RequiredDocuments { get; set; } // This can be a JSON string
    }
}
