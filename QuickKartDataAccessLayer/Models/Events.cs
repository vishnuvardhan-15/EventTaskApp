using System;
using System.Collections.Generic;

namespace EventDataAccessLayer.Models
{
    public partial class Events
    {
        public long EventId { get; set; }
        public string CompanyName { get; set; }
        public long CompanyId { get; set; }
        public string JobName { get; set; }
        public long JobId { get; set; }
        public string EventType { get; set; }
        public long FundValue { get; set; }
        public string EventTriggerType { get; set; }
        public string EventTriggeredBy { get; set; }
        public string PaymentStatus { get; set; }
        public bool RefundStatus { get; set; }
        public string UserComments { get; set; }
        public DateTime TimeStampValue { get; set; }
       
    }
}
