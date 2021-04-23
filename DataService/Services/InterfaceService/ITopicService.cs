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
        Task<int> AddTopic(string Name, string Description, string NameEng, string DescriptionEng, string Image, DateTime StartDate, string Username);

        //Cập nhập Topic
        Task<int> UpdateTopic(int id, string Name, string Description, string NameEng, string Descriptioneng, string Image, DateTime StartDate, string Status);


        //Xóa 1 topic
        Task<int> DeleteTopic(int id);


        //Get tất cả các Topic for Admin
        List<TopicResponse> GetAllTopics();


        //active 1 topic
        Task<int> UpdateStatusFromWatingToActive(int id);

        //thêm 1 obj vào 1 topic
        Task<int> AddExhibitToTopic(int topicId, int exhibitId);


        //lấy những topic chưa đc set room
        List<TopicResponse> GetTopicHasNoRoom();

        //search by name topic for admin
        List<TopicResponse> SearchTopicForAdmin(string name);


        //------------------------------------------------------------------------------------------------------------------------------//
        //------------------------------------------------------------------------------------------------------------------------------//

        //Get all topics for user
        List<TopicResponse> GetAllTopicsForUser();

        //Show highlight topic with rating > 4
        List<TopicResponse> GetHightLightTopic();

        //Get topic by id
        Task<TopicResponse> GetTopicById(int id);







    }
}
