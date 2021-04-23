using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class ExhibitInTopic
    {
        public int ExhibitId { get; set; }
        public int TopicId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }

        public virtual Exhibit Exhibit { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
