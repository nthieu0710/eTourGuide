using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class TopicInRoom
    {
        public int TopicId { get; set; }
        public int RoomId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Room Room { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
