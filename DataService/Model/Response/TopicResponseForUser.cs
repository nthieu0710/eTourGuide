using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class TopicResponseForUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        //public DateTime CreateDate { get; set; }
        public DateTime StartDate { get; set; }
        public float Rating { get; set; }
        //public string Status { get; set; }
        //public Boolean isDelete { get; set; }
    }
}
