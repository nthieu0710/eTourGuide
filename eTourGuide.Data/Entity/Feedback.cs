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
        public string VisitorName { get; set; }
        public double? Rating { get; set; }
        public string Description { get; set; }
        public DateTime? DateTime { get; set; }
        public bool? Status { get; set; }

        public virtual Event Event { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
