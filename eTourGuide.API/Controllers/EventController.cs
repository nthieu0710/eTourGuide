using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.API.Models.Requests;
using eTourGuide.Data.Entity;
using eTourGuide.Service.Exceptions;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTourGuide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }


        //Controller for Add Event
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<ActionResult<int>> InsertEvent([FromBody] PostEventRequest model)
        {

            var rs = await _eventService.AddEvent(model.Name, model.Description, model.NameEng, model.DescriptionEng, model.Image, model.StartDate, model.EndDate);
            return Ok(rs);
           
        }

        //Controller for Add Exhibit To Event
        [Authorize(Roles = "1")]
        [HttpPost("add/exhibit/to/event")]
        public async Task<ActionResult<int>> AddExhibitToEventForAdmin([FromBody] PostExhibitInEventRequest model)
        {
            var rs = await _eventService.AddExhibitToEvent(model.EventId, model.ExhibitId);
            return Ok(rs);

        }


        //Controller for Update Event
        [Authorize(Roles = "1")]
        [HttpPut("id={id}")]
        public async Task<ActionResult<int>> UpdateEvent([FromBody] PutEventRequest model, int id)
        {

            var rs = await _eventService.UpdateEvent(id, model.Name, model.Description, model.NameEng, model.DescriptionEng, model.Image, model.Status, model.StartDate, model.EndDate);
            return Ok(rs);

        }


        //Controller for Active Event
        [Authorize(Roles = "1")]
        [HttpPut("active/event/id={id}")]
        public async Task<ActionResult<int>> ActiveEvent(int id)
        {

            var rs = await _eventService.UpdateStatusFromWatingToActive(id);
            return Ok(rs);

        }


        //Controller for Delete Event
        [Authorize(Roles = "1")]
        [HttpDelete("id={id}")]
        public async Task<ActionResult<int>> DeleteEvent(int id)
        {

            var rs = await _eventService.DeleteEvent(id);
            return Ok(rs);

        }

        //Controller for Get All Events
        [Authorize(Roles = "1")]
        [HttpGet("getAllEvent/Admin")]
        public ActionResult<List<EventResponse>> GetAllEventsForAdmin()
        {

            var rs = _eventService.GetAllEventsForAdmin();
            return Ok(rs);

        }

             

        //Controller for Get Event is not in any Room
        [Authorize(Roles = "1")]
        [HttpGet("get/event/has/no/room")]
        public ActionResult<List<EventResponse>> GetEventThatHasNoRoom()
        {

            var rs = _eventService.GetEventHasNoRoom();
            return Ok(rs);

        }


        //Controller for Search by Event Name Admin
        [Authorize(Roles = "1")]
        [HttpGet("search-event-by-name-for-admin")]
        public ActionResult<List<EventResponse>> SearchEventByNameForAdmin(string name)
        {

            var rs = _eventService.SearchEventForAdmin(name);
            return Ok(rs);

        }

        //=================================================================================================//


        //Controller for Get Highlight Event Rating > 4 for User
        [HttpGet("suggest/highlight/event")]
        public ActionResult<List<EventResponse>> GetHighLightEvent()
        {
         
            var rs = _eventService.GetListHightLightEvent();
            return Ok(rs);

        }

        //Controller for Get Event is Active now for User
        [HttpGet("current/event")]
        public ActionResult<List<EventResponse>> GetCurrentEvent()
        {       
            var rs = _eventService.GetCurrentEvent();
            return Ok(rs);
           
        }

        //Controller for Get All Events For User
        [HttpGet]
        public ActionResult<List<EventResponse>> GetAllEventsForUser()
        {          
            var rs = _eventService.GetAllEventsForUser();
            return Ok(rs);
            
        }

     
        
    }
}
