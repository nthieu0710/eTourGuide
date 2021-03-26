using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Map
    {
        public Map()
        {
            Floors = new HashSet<Floor>();
            Positions = new HashSet<Position>();
        }

        public int Id { get; set; }
        public int? TotalPositions { get; set; }

        public virtual ICollection<Floor> Floors { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
    }
}
