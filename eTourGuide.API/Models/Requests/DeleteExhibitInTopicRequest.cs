﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTourGuide.API.Models.Requests
{
    public class DeleteExhibitInTopicRequest
    {
        public int TopicId { get; set; }
        public int ExhibitId { get; set; }
    }
}
