﻿using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Room
    {
        public Room()
        {
            EventInRooms = new HashSet<EventInRoom>();
            ExhibitInEvents = new HashSet<ExhibitInEvent>();
            ExhibitInTopics = new HashSet<ExhibitInTopic>();
            Positions = new HashSet<Position>();
            TopicInRooms = new HashSet<TopicInRoom>();
        }

        public int Id { get; set; }
        public int? Floor { get; set; }
        public int? No { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<EventInRoom> EventInRooms { get; set; }
        public virtual ICollection<ExhibitInEvent> ExhibitInEvents { get; set; }
        public virtual ICollection<ExhibitInTopic> ExhibitInTopics { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<TopicInRoom> TopicInRooms { get; set; }
    }
}