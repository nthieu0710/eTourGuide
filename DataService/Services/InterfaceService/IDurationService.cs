using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IDurationService
    {
        List<ExhibitFeedbackResponse> DurationForEvent(int id, TimeSpan time);

        List<ExhibitFeedbackResponse> DurationForTopic(int id, TimeSpan time);
    }
}
