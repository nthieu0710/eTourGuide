using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IDurationService
    {

        List<ExhibitFeedbackResponse> SuggestExhibitFromDuration(TimeSpan time);

        TimeSpan GetTotalTimeForVisitExhibitInEvent(int id, int[] exhibitId);

        TimeSpan GetTotalTimeForVisitExhibitInTopic(int id, int[] exhibitId);



        //List<ExhibitFeedbackResponse> DurationForEvent(int id, TimeSpan time);

        //List<ExhibitFeedbackResponse> DurationForTopic(int id, TimeSpan time);
    }
}
