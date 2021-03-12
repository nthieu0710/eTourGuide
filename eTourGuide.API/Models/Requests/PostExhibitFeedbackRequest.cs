using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTourGuide.API.Models.Requests
{
    public class PostExhibitFeedbackRequest
    {
        //int exhibitId, string visitorName, double rating, string description
        public int ExhibitId { get; set; }
        public string VisitorName{ get; set; }

        public double Rating{ get; set; }
        public string Description { get; set; }
    }
}
