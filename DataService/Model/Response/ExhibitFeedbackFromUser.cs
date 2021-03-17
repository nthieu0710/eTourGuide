using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class ExhibitFeedbackFromUser
    {
        public int Id { get; set; }
        public int ExhibitId { get; set; }
        public string ExhibitName { get; set; }
        public string VisitorName { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public bool Status { get; set; }
    }
}
