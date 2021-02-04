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

        Task<Event> AddEvent(string Name, string Description, string Image, DateTime StartDate, DateTime EndDate);

        Task<Event> UpdateEvent(int id, string Name, string Description, string Image, string Status, DateTime StartDate, DateTime EndDate);
        Task<Event> DeleteEvent(int id);

        Task<int> UpdateStatusFromWatingToActive(int id);
    }
}
