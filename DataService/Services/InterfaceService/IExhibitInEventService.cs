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
        List<ExhibitResponse> GetExhibitInEvent(int id);


        List<ExhibitResponse> GetExhibitInEventForAdmin(int id);

        Task<int> DeleteExhibitInEvent(int exhibitId);
    }
}
