using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTourGuide.API.Models.Requests
{
    public class GetDurationRequest
    {
        public int Id { get; set; }
        public TimeSpan Time { get; set; }
    }
}
