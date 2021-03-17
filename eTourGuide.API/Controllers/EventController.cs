using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.API.Models.Requests;
using eTourGuide.Data.Entity;
using eTourGuide.Service.Exceptions;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
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


        //Controller for Insert Event
        [HttpPost]
        public async Task<ActionResult<int>> InsertEvent([FromBody] PostEventRequest model)
        {

           /* try
            {*/
                var rs = await _eventService.AddEvent(model.Name, model.Description, model.Image, model.StartDate, model.EndDate);
                return Ok(rs);
            /*}
            catch (Exception)
            {

                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Add Exhibit to Topic Error!!!");
            }*/

        }

        //Get Highlight Event Rating > 4
        [HttpGet("suggest/highlight/event")]
        public ActionResult<List<EventResponse>> GetHighLightEvent()
        {
           /* try
            {*/
                var rs = _eventService.GetListHightLightEvent();
                return Ok(rs);
           /* }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Highlight Event Error!!!");
            }*/
        }

        
        [HttpGet("current/event")]
        public ActionResult<List<EventResponse>> GetCurrentEvent()
        {
           /* try
            {*/
                var rs = _eventService.GetCurrentEvent();
                return Ok(rs);
            /*}
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Highlight Event Error!!!");
            }*/
        }


        [HttpGet]
        public ActionResult<List<EventResponse>> GetAllEventsForUser()
        {
           /* try
            {*/
                var rs = _eventService.GetAllEventsForUser();
                return Ok(rs);
            /*}
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Events Error!!!");
            }*/
        }



        [HttpGet("getAllEvent/Admin")]
        public ActionResult<List<EventResponse>> GetAllEventsForAdmin()
        {
           /* try
            {*/
                var rs = _eventService.GetAllEventsForAdmin();
                return Ok(rs);
           /* }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Events Error!!!");
            }*/
        }

        [HttpPut("id={id}")]
        public async Task<ActionResult<int>> UpdateEvent([FromBody] PutEventRequest model, int id)
        {

          /*  try
            {*/
                var rs = await _eventService.UpdateEvent(id, model.Name, model.Description, model.Image, model.Status, model.StartDate, model.EndDate);
                return Ok(rs);
            /*}
            catch (Exception)
            {

                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Add Exhibit to Topic Error!!!");
            }*/
        }

        [HttpPut("active/event/id={id}")]
        public async Task<ActionResult<int>> ActiveEvent(int id)
        {

          
                var rs = await _eventService.UpdateStatusFromWatingToActive(id);
                return Ok(rs);
           
        }

        [HttpDelete("id={id}")]
        public async Task<ActionResult<int>> DeleteEvent(int id)
        {

           /* try
            {*/
                var rs = await _eventService.DeleteEvent(id);
                return Ok(rs);
            /*}
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Add Exhibit to Topic Error!!!");
            }*/
        }



        [HttpPost("add/exhibit/to/event")]
        public async Task<ActionResult<int>> AddExhibitToEventForAdmin([FromBody] PostExhibitInEventRequest model)
        {
            /*try
            {*/
                var rs = await _eventService.AddExhibitToEvent(model.EventId, model.ExhibitId);
                return Ok(rs);
            /*}
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Add Exhibit to Event Error!!!");
            }*/
        }

        [HttpGet("get/event/has/no/room")]
        public ActionResult<List<EventResponse>> GetEventThatHasNoRoom()
        {
           /* try
            {*/
                var rs = _eventService.GetEventHasNoRoom();
                return Ok(rs);
           /* }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Events Error!!!");
            }*/
        }


        //Search event by name
        [HttpGet("search-event-by-name-for-admin")]
        public ActionResult<List<EventResponse>> SearchEventByNameForAdmin(string name)
        {
           /* try
            {*/
                var rs = _eventService.SearchEventForAdmin(name);
                return Ok(rs);
           /* }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not find !!!");
            }*/
        }
    }
}
