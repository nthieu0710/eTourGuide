using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IDurationService
    {
        //Nhập vào tgian có hạn của visitor => suggest ra list exhibit có thể đi
        List<ExhibitResponse> SuggestExhibitFromDuration(TimeSpan time);


        //tổng thời gian để dừng lại xem các exhibit ở 1 event
        TimeSpan GetTotalTimeForVisitExhibitInEvent(int id, int[] exhibitId);
        //tổng thời gian để dừng lại xem các exhibi ở 1 topic
        TimeSpan GetTotalTimeForVisitExhibitInTopic(int id, int[] exhibitId);


        //tính tổng quãng đường di chuyển giữa các room 
        Double TotalDistance(int[] room);




        //tổng thời gian di chuyển + dừng lại xem hiện vật ở 1 topic
        TimeSpan TotalTimeForVisitorInTopic(int id, int[] exhibitId);

        //tổng thời gian di chuyển + dừng lại xem hiện vật ở 1 event
        TimeSpan TotalTimeForVisitorInEvent(int id, int[] exhibitId);



        List<int> SuggestRouteBaseOnExhibit(int[] exhibitId);



    }
}
