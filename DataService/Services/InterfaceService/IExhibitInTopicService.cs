using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IExhibitInTopicService
    {
        List<ExhibitFeedbackResponse> GetExhibitInTopic(int id);
    }
}
