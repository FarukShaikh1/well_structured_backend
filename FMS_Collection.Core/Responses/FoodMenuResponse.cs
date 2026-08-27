using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class FoodMenuResponse : CommonResponse
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public int MenuDate { get; set; }

        public int Sequence { get; set; }

        public string? Breakfast { get; set; }

        public string? Lunch { get; set; }

        public string? EveningBreakfast { get; set; }

        public string? Dinner { get; set; }
    }
}