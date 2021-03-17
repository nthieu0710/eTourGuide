using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class TopicResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string CreateDate { get; set; }
        public string StartDate { get; set; }
        public float Rating { get; set; }
        public string Status { get; set; }
        public Boolean isDelete { get; set; }
        public int TotalFeedback { get; set; }

    }
}
