using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class ProductChannel
    {
        public int ProductChannelMappingId { get; set; }
        public int ProductChannelID { get; set; }
        public string ProductChannelName { get; set; } = string.Empty;
        public string ProductChannelDescription { get; set; } = string.Empty;
    }
}
