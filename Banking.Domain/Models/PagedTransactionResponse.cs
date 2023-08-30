
namespace Banking.Domain.Models
{
    public class PagedTransactionResponse
    {
        public List<Transaction>? Transactions { get; set; }
        public int TotalCount { get; set; }
    }

}
