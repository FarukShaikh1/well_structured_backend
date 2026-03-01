using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Entities
{
    public class Budget : CommonResponse
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public string PayTo { get; set; }
        public string? Purpose { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
    }
}