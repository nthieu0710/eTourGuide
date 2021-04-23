using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IDurationService
    {
        //Nhập vào tgian có hạn của visitor => suggest ra list exhibit có thể xem for user
        Task<List<ExhibitResponse>> SuggestExhibitFromDuration(TimeSpan time);


        //tổng thời gian di chuyển + dừng lại xem hiện vật ở 1 topic
        Task<TimeSpan> TotalTimeForVisitorInTopic(int id, List<int> exhibitId);


        //tổng thời gian di chuyển + dừng lại xem hiện vật ở 1 event
        Task<TimeSpan> TotalTimeForVisitorInEvent(int id, List<int> exhibitId);       


        //trả về list đường đi và text hướng dẫn đi cho user khi họ chọn exhibit
        Task<List<SuggestRouteResponse>> GetRouteBaseOnExhibit(List<int> exhibitId);

      

    }
}
