using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IExhibitInTopicService
    {
        List<ExhibitResponse> GetExhibitInTopic(int id);

        List<ExhibitResponse> GetExhibitInTopicForAdmin(int id);

        Task<int> DeleteExhibitIntTopic(int exhibitId);

       
    }
}
