using eTourGuide.Data.Entity;
using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IFeedbackService
    {
        //Get list feedback(exhibit/event/topic)
        List<Feedback> GetFeedbacksExhibitcForUserById(int Id);

        List<Feedback> GetFeedbacksTopicForUserById(int Id);

        List<Feedback> GetFeedbacksEventForUserById(int Id);



        Task<int> CreateUserFeedbackForExhibit(int exhibitId, string visitorName, double rating, string description);
        Task<int> CreateUserFeedbackForEvent(int eventId, string visitorName, double rating, string description);
        Task<int> CreateUserFeedbackForTopic(int topicId, string visitorName, double rating, string description);



        List<EventFeedbackFromUser> GetFeedbacksEventForAdmin();

        List<ExhibitFeedbackFromUser> GetFeedbacksExhibitForAdmin();

        List<TopicFeedbackFromUser> GetFeedbacksTopicForAdmin();

      





    }
}
