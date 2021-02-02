﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class ExhibitFeedbackResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double Rating { get; set; }
        public int TotalFeedback { get; set; }

    }
}
