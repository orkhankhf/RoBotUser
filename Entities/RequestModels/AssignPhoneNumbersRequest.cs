namespace Entities.RequestModels
{
    public class AssignPhoneNumbersRequest
    {
        public List<int> Cities { get; set; }
        public List<int> Categories { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
    }

    public class AssignPhoneNumbersResponse
    {
        public TimeSpan? RemainingWaitTime { get; set; }
    }
}
