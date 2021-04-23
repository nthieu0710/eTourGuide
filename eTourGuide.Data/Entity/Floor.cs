using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Floor
    {
        public Floor()
        {
            Positions = new HashSet<Position>();
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }
        public int FloorNo { get; set; }
        public int MapId { get; set; }
        public string Image { get; set; }

        public virtual Map Map { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
