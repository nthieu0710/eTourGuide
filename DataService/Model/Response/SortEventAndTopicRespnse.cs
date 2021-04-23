using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class SortEventAndTopicRespnse
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public int RoomId { get; set; }
        public string Type { get; set; }
    }
}
