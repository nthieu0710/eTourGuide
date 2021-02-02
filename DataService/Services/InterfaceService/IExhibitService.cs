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
        //Task<Exhibit> AddExhibit(string Name, string Description, string Image, float Rating, int Status);

        List<ExhibitFeedbackResponse> GetHightLightExhibit();

        List<ExhibitResponseForUser> GetAllExhibitsForUser();

        List<ExhibitResponseForUser> GetNewExhibit();
    }
}
