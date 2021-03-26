using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Position
    {
        public Position()
        {
            EdgeFromPositionNavigations = new HashSet<Edge>();
            EdgeToPositionNavigations = new HashSet<Edge>();
        }

        public int Id { get; set; }
        public string DescriptionEng { get; set; }
        public string DescriptionVie { get; set; }
        public int? Type { get; set; }
        public double Dx { get; set; }
        public double Dy { get; set; }
        public int Floor { get; set; }
        public int RoomId { get; set; }
        public int MapId { get; set; }

        public virtual Map Map { get; set; }
        public virtual Room Room { get; set; }
        public virtual ICollection<Edge> EdgeFromPositionNavigations { get; set; }
        public virtual ICollection<Edge> EdgeToPositionNavigations { get; set; }
    }
}
