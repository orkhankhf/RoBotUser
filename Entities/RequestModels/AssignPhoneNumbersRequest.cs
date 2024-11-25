using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestModels
{
    public class AssignPhoneNumbersRequest
    {

    }

    public class AssignPhoneNumbersResponse
    {
        public TimeSpan? RemainingWaitTime { get; set; }
    }
}
