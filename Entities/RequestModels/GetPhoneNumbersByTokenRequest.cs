using Entities.Models;

namespace Entities.RequestModels
{
    public class GetPhoneNumbersByTokenResponse
    {
        public IEnumerable<PhoneNumber> PhoneNumbers { get; set; }
        public TimeSpan? RemainingWaitTime { get; set; }
    }
}
