using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public int? EventId { get; set; }
        public int? TopicId { get; set; }
        public int? ExhibittId { get; set; }
        public string VisitorName { get; set; }
        public double? Rating { get; set; }
        public string Description { get; set; }
        public DateTime? DateTime { get; set; }

        public virtual Event Event { get; set; }
        public virtual Exhibit Exhibitt { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
