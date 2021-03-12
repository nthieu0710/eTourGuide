using eTourGuide.Data.Entity;
using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IRoomService
    {
        List<RoomResponse> GetRoomForExhibit(int[] exhibitId);

        List<ExhibitFeedbackResponse> GetExhibitFromRoom(int roomId);

        List<RoomResponse> GetAllRoom();

        Task<int> AddTopicToRoom(int topicId, int roomId);
        Task<int> AddEventToRoom(int eventId, int roomId);



        Task<int> DeleteTopicInRoom(int topicId);
        Task<int> DeleteEventInRoom(int eventId);



        Task<ObjectResponseInRoomForAdmin> GetTopicOrEventInRoom(int roomId);
        

    }
}
