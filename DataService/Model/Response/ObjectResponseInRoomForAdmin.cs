﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class ObjectResponseInRoomForAdmin
    {
        public int RoomId { get; set; }
        public int EventOrTopicId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string NameEng { get; set; }
        public string DescriptionEng { get; set; }
        public string Image{ get; set; }
        public string CreateDate { get; set; }
        public double Rating { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Type { get; set; }
    }
}
