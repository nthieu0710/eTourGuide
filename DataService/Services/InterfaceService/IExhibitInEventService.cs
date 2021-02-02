﻿using eTourGuide.Data.Entity;
using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IExhibitInEventService
    {
        List<ExhibitFeedbackResponse> GetExhibitInEvent(int id);
    }
}
