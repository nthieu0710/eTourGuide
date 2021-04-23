using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.API.Models.Requests;
using eTourGuide.Data.Entity;
using eTourGuide.Service.Exceptions;
using eTourGuide.Service.Helpers;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTourGuide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExhibitInEventController : ControllerBase
    {
        private readonly IExhibitInEventService _exhibitInEventService;

        public ExhibitInEventController(IExhibitInEventService exhibitInEventService)
        {
            _exhibitInEventService = exhibitInEventService;
        }



        //Controller for Delete Exhibit In Event for Admin
        [Authorize(Roles = AccountRole.Admin)]
        [HttpDelete("delete/exhibit/in/event/id={id}")]
        public async Task<ActionResult<int>> DeleteExhibitInEventForAdminAsync(int id)
        {
           
            var rs = await _exhibitInEventService.DeleteExhibitInEvent(id);
            return Ok(rs);
          
        }


        //Controller for get Exhibit in Event for Admin
        [Authorize(Roles = AccountRole.Admin)]
        [HttpGet("get/exhibit/in/event/for/admin")]
        public ActionResult<List<ExhibitResponse>> GetExhibitInEventForAdmin(int eventId)
        {
        
            var rs = _exhibitInEventService.GetExhibitInEventForAdmin(eventId);
            return Ok(rs);
    
        }

        //Controller for get Exhibit in CLOSED Event for user
        [Authorize(Roles = AccountRole.Admin)]
        [HttpGet("get/exhibit/in/closed/event/for/admin")]
        public ActionResult<List<ExhibitResponse>> GetExhibitInClosedEventForAdmin(int eventId)
        {
            
            var rs = _exhibitInEventService.GetExhbitForClosedEvent(eventId);
            return Ok(rs);
           
        }


        //================================================================================//

        //Controller for get Exhibit in Event for user
        [HttpGet("get/exhibit/in/event/for/user")]
        public ActionResult<List<ExhibitResponse>> GetExhibitInEvent(int eventId)
        {
           
            var rs = _exhibitInEventService.GetExhibitInEvent(eventId);
            return Ok(rs);
        }
    }
}
