using System;
using System.Collections.Generic;

#nullable disable

namespace eTourGuide.Data.Entity
{
    public partial class Account
    {
        public Account()
        {
            Events = new HashSet<Event>();
            Exhibits = new HashSet<Exhibit>();
            Topics = new HashSet<Topic>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Exhibit> Exhibits { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
    }
}
