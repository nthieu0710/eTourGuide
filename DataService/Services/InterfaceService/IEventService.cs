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
        List<EventResponse> GetAllEventsForAdmin();
        List<EventFeedbackResponse> GetListHightLightEvent();
        List<EventResponseForUser> GetAllEventsForUser();

        List<EventFeedbackResponse> GetCurrentEvent();

        Task<int> AddEvent(string Name, string Description, string Image, DateTime StartDate, DateTime EndDate);

        Task<int> UpdateEvent(int id, string Name, string Description, string Image, string Status, DateTime StartDate, DateTime EndDate);
        Task<int> DeleteEvent(int id);

        Task<int> UpdateStatusFromWatingToActive(int id);


        Task<int> AddExhibitToEvent(int eventId, int exhibitId);

        List<EventResponse> GetEventHasNoRoom();
    }
}
