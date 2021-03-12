using eTourGuide.Data.Entity;
using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface ITopicService
    {
        //Tạo mới 1 Topic
        Task<int> AddTopic(string Name, string Description, string Image, DateTime StartDate);

        //Get tất cả các Topic
        List<TopicResponse> GetAllTopics();

        //Cập nhập Topic
        Task<int> UpdateTopic(int id, string Name, string Description, string Image, DateTime StartDate ,string Status);


        Task<int> DeleteTopic(int id);


        //show highlight topic with rating > 4
        List<TopicFeedbackResponse> GetHightLightTopic();

        Task<TopicResponse> GetTopicById(int id);

        List<TopicResponseForUser> GetAllTopicsForUser();


        Task<int> UpdateStatusFromWatingToActive(int id);
        

        Task<int> AddExhibitToTopic(int topicId, int exhibitId);



        List<TopicResponse> GetTopicHasNoRoom();
    }
}
