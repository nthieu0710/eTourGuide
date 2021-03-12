using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class EventFeedbackFromUser
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string VisitorName { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
    }
}
