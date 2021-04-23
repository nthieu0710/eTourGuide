using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Edge
    {
        public int Id { get; set; }
        public int FromPosition { get; set; }
        public int ToPosition { get; set; }
        public int Cost { get; set; }
        public string Description { get; set; }

        public virtual Position FromPositionNavigation { get; set; }
        public virtual Position ToPositionNavigation { get; set; }
    }
}
