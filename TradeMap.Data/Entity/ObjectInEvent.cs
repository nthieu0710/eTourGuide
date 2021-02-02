using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class ObjectInEvent
    {
        public int Id { get; set; }
        public int? ObjectId { get; set; }
        public int? EventId { get; set; }
        public int? RoomId { get; set; }
        public DateTime? CreateDate { get; set; }
        public double? PriorityLevel { get; set; }

        public virtual Event Event { get; set; }
        public virtual Object Object { get; set; }
        public virtual Room Room { get; set; }
    }
}
