using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Helpers
{
    public static class EventStatus
    {
        public enum Status
        {
            New = 0,
            Waiting = 1,
            Active = 2,
            Disactive = 3,
            Closed = 4
        }
    }
}
