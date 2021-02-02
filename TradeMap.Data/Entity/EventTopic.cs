using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class EventTopic
    {
        public int Id { get; set; }
        public int? EventId { get; set; }
        public int? TopicId { get; set; }

        public virtual Event Event { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
