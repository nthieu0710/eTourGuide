using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class ExhibitResponseForUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        //public DateTime? CreateDate { get; set; }
        public double Rating { get; set; }
        //public int? Status { get; set; }
        //public TimeSpan? Duration { get; set; }
    }
}
