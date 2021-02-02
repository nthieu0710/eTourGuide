using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Object
    {
        public Object()
        {
            Feedbacks = new HashSet<Feedback>();
            ObjectInEvents = new HashSet<ObjectInEvent>();
            ObjectInTopics = new HashSet<ObjectInTopic>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime? CreateDate { get; set; }
        public double? Rating { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<ObjectInEvent> ObjectInEvents { get; set; }
        public virtual ICollection<ObjectInTopic> ObjectInTopics { get; set; }
    }
}
