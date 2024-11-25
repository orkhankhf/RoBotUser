using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestModels
{
    public class GetAssignedPhoneNumberStatisticsRequest
    {

    }

    public class GetAssignedPhoneNumberStatisticsResponse
    {
        public int WaitingForSendingMessageCount { get; set; }
        public int SentMessageNumberCount { get; set; }
        public DateTime? LastAssignTime { get; set; }
        public int LimitPerRequest { get; set; }
    }
}
