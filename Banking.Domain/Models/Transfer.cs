using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Models
{
    public class Transfer
    {
        public int Id { get; set; }
        public int FromAccountId { get; set; }
        public virtual Account FromAccount { get; set; }
        public int ToAccountId { get; set; }
        public virtual Account ToAccount { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}

