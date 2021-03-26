using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Room
    {
        public Room()
        {
            Events = new HashSet<Event>();
            Positions = new HashSet<Position>();
            Topics = new HashSet<Topic>();
        }

        public int Id { get; set; }
        public int? Floor { get; set; }
        public int? No { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
    }
}
