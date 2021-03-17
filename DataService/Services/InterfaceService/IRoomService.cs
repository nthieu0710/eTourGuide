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
        //dựa vào các exhibit user chọn để get ra đc phòng của chúng
        List<RoomResponse> GetRoomForExhibit(int[] exhibitId);

        //dưa vào phòng đã chọn để show tất cả exhibit trong đó
        List<ExhibitResponse> GetExhibitFromRoom(int roomId);

        List<RoomResponse> GetAllRoom();

        Task<int> AddTopicToRoom(int topicId, int roomId);
        Task<int> AddEventToRoom(int eventId, int roomId);



        Task<int> DeleteTopicInRoom(int topicId);
        Task<int> DeleteEventInRoom(int eventId);



        Task<ObjectResponseInRoomForAdmin> GetTopicOrEventInRoom(int roomId);
        

    }
}
