using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestModels
{
    public class GetFilteredPhoneNumbersRequest
    {
        public List<int> Cities { get; set; }
        public List<int> Categories { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
    }

    public class GetFilteredPhoneNumbersResponse
    {
        public int Count { get; set; }
    }
}
