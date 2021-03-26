using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Event
    {
        public Event()
        {
            ExhibitInEvents = new HashSet<ExhibitInEvent>();
            Feedbacks = new HashSet<Feedback>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string NameEng { get; set; }
        public string DescriptionEng { get; set; }
        public string Image { get; set; }
        public DateTime? CreateDate { get; set; }
        public double? Rating { get; set; }
        public int? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsDelete { get; set; }
        public int? RoomId { get; set; }
        public string UserName { get; set; }

        public virtual Room Room { get; set; }
        public virtual Account UserNameNavigation { get; set; }
        public virtual ICollection<ExhibitInEvent> ExhibitInEvents { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
