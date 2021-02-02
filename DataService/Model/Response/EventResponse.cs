using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class EventResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string CreateDate { get; set; }
        public double Rating { get; set; }
        public String Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Boolean isDelete { get; set; }
    }
}
