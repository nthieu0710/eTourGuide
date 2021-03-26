using System;
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
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string NameEng { get; set; }
        public string DescriptionEng { get; set; }
        public string Image { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool? IsDelete { get; set; }
        public string Username { get; set; }

        public virtual ICollection<ExhibitInEvent> ExhibitInEvents { get; set; }
        public virtual ICollection<ExhibitInTopic> ExhibitInTopics { get; set; }
    }
}
