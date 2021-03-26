using eTourGuide.Data.Entity;
using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IExhibitInEventService
    {
        //xóa 1 hiện vật ra khỏi event
        Task<int> DeleteExhibitInEvent(int exhibitId);

        //lấy danh sách hiện vật trong 1 topic closed
        List<ExhibitResponse> GetExhbitForClosedEvent(int eventId);

        //lấy danh sách những hiện vật có trong 1 topic
        List<ExhibitResponse> GetExhibitInEventForAdmin(int id);

        //------------------------------------------------------------------------------------------------------------------------------//
        //------------------------------------------------------------------------------------------------------------------------------//


        //lấy danh sách những hiện vật có trong 1 topic
        List<ExhibitResponse> GetExhibitInEvent(int id);


    }
}
