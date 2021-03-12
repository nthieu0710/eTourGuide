using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTourGuide.API.Models.Requests
{
    public class PostTopicFeedbackRequest
    {
        public int TopicId { get; set; }
        public string VisitorName { get; set; }

        public double Rating { get; set; }
        public string Description { get; set; }
    }
}
