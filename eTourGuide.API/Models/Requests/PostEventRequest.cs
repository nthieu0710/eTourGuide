using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTourGuide.API.Models.Requests
{
    public class PostEventRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string NameEng { get; set; }
        public string DescriptionEng { get; set; }
        public string Image { get; set; }
        //public DateTime CreateDate { get; set; }
        //public float Rating { get; set; }
        //public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public Boolean isDelete { get; set; }
    }
}
