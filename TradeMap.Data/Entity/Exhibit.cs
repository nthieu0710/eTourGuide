﻿using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Exhibit
    {
        public Exhibit()
        {
            ExhibitInEvents = new HashSet<ExhibitInEvent>();
            ExhibitInTopics = new HashSet<ExhibitInTopic>();
            Feedbacks = new HashSet<Feedback>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime? CreateDate { get; set; }
        public double? Rating { get; set; }
        public int? Status { get; set; }
        public TimeSpan? Duration { get; set; }

        public virtual ICollection<ExhibitInEvent> ExhibitInEvents { get; set; }
        public virtual ICollection<ExhibitInTopic> ExhibitInTopics { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}