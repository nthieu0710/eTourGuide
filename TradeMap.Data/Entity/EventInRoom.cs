using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class EventInRoom
    {
        public int EventId { get; set; }
        public int RoomId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Event Event { get; set; }
        public virtual Room Room { get; set; }
    }
}
