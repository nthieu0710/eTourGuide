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
        
        //set room for topic
        Task<int> AddTopicToRoom(int topicId, int roomId);

        //set room for event
        Task<int> AddEventToRoom(int eventId, int roomId);


        //xóa topic ra khỏi phòng
        Task<int> DeleteTopicInRoom(int topicId);

        //xóa event ra khỏi phòng
        Task<int> DeleteEventInRoom(int eventId);


        //khi admin click vào 1 room thì sẽ hiện topic or event đang chứa trong room đó
        Task<ObjectResponseInRoomForAdmin> GetTopicOrEventInRoom(int roomId);


        //------------------------------------------------------------------------------------------------------------------------------//
        //------------------------------------------------------------------------------------------------------------------------------//


        //dựa vào các exhibit user chọn để get ra đc phòng của chúng
        List<RoomResponse> GetRoomForExhibit(int[] exhibitId);

        //dưa vào phòng đã chọn để show tất cả exhibit trong đó
        List<ExhibitResponse> GetExhibitFromRoom(int roomId);

        //lấy ra những phòng đang có chứa event or topic
        List<RoomResponse> GetAllRoom();









        List<RoomResponse> GetRoomFromListExhibit(List<int> exhibitId);

    }
}
