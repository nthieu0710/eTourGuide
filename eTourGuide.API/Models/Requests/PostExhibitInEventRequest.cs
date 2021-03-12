using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTourGuide.API.Models.Requests
{
    public class PostExhibitInEventRequest
    {
        public int EventId { get; set; }

        public int ExhibitId { get; set; }
    }
}
