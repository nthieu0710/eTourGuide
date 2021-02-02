using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Position
    {
        public int PointId { get; set; }
        public int? RoomId { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }

        public virtual Room Room { get; set; }
    }
}
