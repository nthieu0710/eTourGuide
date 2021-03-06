﻿using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class ExhibitInEvent
    {
        public int ExhibitId { get; set; }
        public int EventId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }

        public virtual Event Event { get; set; }
        public virtual Exhibit Exhibit { get; set; }
    }
}
