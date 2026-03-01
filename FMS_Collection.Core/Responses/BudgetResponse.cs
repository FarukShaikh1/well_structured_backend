
using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class BudgetResponse : CommonResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string PayTo { get; set; }
        public string Purpose { get; set; }

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }   // From TransactionCategories

        public decimal Amount { get; set; }

    }
}
