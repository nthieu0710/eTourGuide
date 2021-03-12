using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTourGuide.API.Models.Requests
{
    public class PostTopicInRoomRequest
    {
        public int TopicId { get; set; }
        public int RoomId { get; set; }
    }
}
