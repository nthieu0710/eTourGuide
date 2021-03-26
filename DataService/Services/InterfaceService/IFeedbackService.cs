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
        
        //get list feedback cho admin
        List<EventFeedbackFromUser> GetFeedbacksEventForAdmin();

        //List<ExhibitFeedbackFromUser> GetFeedbacksExhibitForAdmin();

        List<TopicFeedbackFromUser> GetFeedbacksTopicForAdmin();



        //disable những feedback spam
        Task<int> DisableFeedbackForAdmin(int feedbackID);

        Task<int> EnableFeedbackForAdmin(int feedbackID);


        //------------------------------------------------------------------------------------------------------------------------------//
        //------------------------------------------------------------------------------------------------------------------------------//


        //Tạo 1 phản hồi từ vistitor for 1 event / topic
        Task<int> CreateUserFeedbackForEvent(int eventId, string visitorName, double rating, string description);
        Task<int> CreateUserFeedbackForTopic(int topicId, string visitorName, double rating, string description);



        //list feedback của 1 event/topic
        List<TopicFeedbackFromUser> GetFeedbacksTopicForUserById(int Id);

        List<EventFeedbackFromUser> GetFeedbacksEventForUserById(int Id);





        //Create 1 feedback (exhibit/topic/event) for user 
        //Task<int> CreateUserFeedbackForExhibit(int exhibitId, string visitorName, double rating, string description);
        //Get list feedback(exhibit/event/topic) for user by id của feedback
        //List<ExhibitFeedbackFromUser> GetFeedbacksExhibitcForUserById(int Id);
    }
}
