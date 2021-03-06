﻿using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Topic
    {
        public Topic()
        {
            ExhibitInTopics = new HashSet<ExhibitInTopic>();
            Feedbacks = new HashSet<Feedback>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string NameEng { get; set; }
        public string DescriptionEng { get; set; }
        public string Image { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StartDate { get; set; }
        public double? Rating { get; set; }
        public int Status { get; set; }
        public bool IsDelete { get; set; }
        public int? RoomId { get; set; }
        public string Username { get; set; }

        public virtual Room Room { get; set; }
        public virtual Account UsernameNavigation { get; set; }
        public virtual ICollection<ExhibitInTopic> ExhibitInTopics { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
