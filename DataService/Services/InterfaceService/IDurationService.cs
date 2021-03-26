using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IDurationService
    {
        //Nhập vào tgian có hạn của visitor => suggest ra list exhibit có thể xem for user
        List<ExhibitResponse> SuggestExhibitFromDuration(TimeSpan time);
     

        //tổng thời gian di chuyển + dừng lại xem hiện vật ở 1 topic
        TimeSpan TotalTimeForVisitorInTopic(int id, int[] exhibitId);


        //tổng thời gian di chuyển + dừng lại xem hiện vật ở 1 event
        TimeSpan TotalTimeForVisitorInEvent(int id, int[] exhibitId);       


        //trả về list đường đi và text hướng dẫn đi cho user khi họ chọn exhibit
        List<SuggestRouteResponse> GetRouteBaseOnExhibit(int[] exhibitId);





















        //tính tổng quãng đường di chuyển giữa các room 
        Double TotalDistance(int[] room);

        //List<int> SuggestRouteBaseOnExhibit(int[] exhibitId);

        //string GetTextSuggestRoute(List<int> listNode);


        //tổng thời gian để dừng lại xem các exhibit ở 1 event
        //TimeSpan GetTotalTimeForVisitExhibitInEvent(int id, int[] exhibitId);
        //tổng thời gian để dừng lại xem các exhibi ở 1 topic
        //TimeSpan GetTotalTimeForVisitExhibitInTopic(int id, int[] exhibitId);

    }
}
