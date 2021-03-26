using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IExhibitInTopicService
    {
        
        //get ra những hiện vật đang có trong 1 topic for admin
        List<ExhibitResponse> GetExhibitInTopicForAdmin(int id);

        //xóa 1 hiện vật khỏi topic
        Task<int> DeleteExhibitIntTopic(int exhibitId);

        //lấy những hiện vật trong 1 Closed topic
        List<ExhibitResponse> GetExhbitForClosedTopic(int topicId);

        //------------------------------------------------------------------------------------------------------------------------------//
        //------------------------------------------------------------------------------------------------------------------------------//

        //get những hiện vật có trong 1 topic for user
        List<ExhibitResponse> GetExhibitInTopic(int id);
    }
}
