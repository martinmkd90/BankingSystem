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
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Features { get; set; } = string.Empty; // This can be a JSON string
        public string Benefits { get; set; } = string.Empty;
        public string TermsAndConditions { get; set; } = string.Empty;
        public string EligibilityCriteria { get; set; } = string.Empty;
        public string RequiredDocuments { get; set; } = string.Empty; // This can be a JSON string
    }
}
