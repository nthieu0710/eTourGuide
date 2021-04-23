using eTourGuide.Data.Entity;
using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IEventService
    {
        //thêm mới 1 sự kiện
        Task<int> AddEvent(string Name, string Description, string NameEng, string DescriptionEng, string Image, DateTime StartDate, DateTime EndDate, string Username);

        //cập nhập 1 event
        Task<int> UpdateEvent(int id, string Name, string Description, string NameEng, string DescriptionEng, string Image, string Status, DateTime StartDate, DateTime EndDate);

        //xóa 1 event
        Task<int> DeleteEvent(int id);


        //get all event for admin
        List<EventResponse> GetAllEventsForAdmin();

        //active 1 topicc
        Task<int> UpdateStatusFromWatingToActive(int id);

        //thêm 1 hiện vật vào event
        Task<int> AddExhibitToEvent(int eventId, int exhibitId);

        //lấy những event chưa đc setup phòng
        List<EventResponse> GetEventHasNoRoom();

        //tìm kiếm event theo name
        List<EventResponse> SearchEventForAdmin(string name);


        //------------------------------------------------------------------------------------------------------------------------------//
        //------------------------------------------------------------------------------------------------------------------------------//

        //get những event đang khả dụng mà có rating cao > 4
        List<EventResponse> GetListHightLightEvent();

        //get tất cả event đang khả dụng for user
        List<EventResponse> GetAllEventsForUser();

        //lấy những event đang active cho user
        List<EventResponse> GetCurrentEvent();

        
    }
}
