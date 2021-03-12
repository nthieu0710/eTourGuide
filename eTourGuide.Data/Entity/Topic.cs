using System;
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
            TopicInRooms = new HashSet<TopicInRoom>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public double? Rating { get; set; }
        public int? Status { get; set; }
        public bool? IsDelete { get; set; }

        public virtual ICollection<ExhibitInTopic> ExhibitInTopics { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<TopicInRoom> TopicInRooms { get; set; }
    }
}
