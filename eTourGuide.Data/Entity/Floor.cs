using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Floor
    {
        public int FloorId { get; set; }
        public int? MapId { get; set; }
        public string Image { get; set; }

        public virtual Map Map { get; set; }
    }
}
