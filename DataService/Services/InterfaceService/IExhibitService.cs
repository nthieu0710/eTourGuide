using eTourGuide.Data.Entity;
using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IExhibitService
    {
        Task<int> AddExhibit(string Name, string Description, string Image, TimeSpan duration);

        Task<int> UpdateExhibit(int id, string Name, string Description, string Image, TimeSpan Duration);

        Task<int> DeleteExhibit(int id);

        List<ExhibitResponse> GetHightLightExhibit();

        List<ExhibitResponse> GetAllExhibitsForUser();

        List<ExhibitResponse> GetNewExhibit();

        List<ExhibitResponse> GetAvailableExhibit();

        List<ExhibitResponse> GetAllExhibitForAdmin();

        List<ExhibitResponse> SearchExhibitForAdmin(string name);
    }
}
