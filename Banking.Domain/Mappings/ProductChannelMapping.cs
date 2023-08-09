using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Banking.Domain.Models;

namespace Banking.Domain.Mappings
{
    public class ProductChannelMapping
    {
        public int ProductChannelID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; } = new Product(); // Navigation property
        public int ChannelID { get; set; }
        public ProductChannel Channel { get; set; } = new ProductChannel(); // Navigation property
    }
}
