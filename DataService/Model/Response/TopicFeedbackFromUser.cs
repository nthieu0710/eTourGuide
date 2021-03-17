using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class TopicFeedbackFromUser
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string VisitorName { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public bool Status { get; set; }
    }
}
