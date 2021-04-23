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
    public class DurationController : ControllerBase
    {
        private readonly IDurationService _durationService;

        public DurationController(IDurationService durationService)
        {
            _durationService = durationService;
        }

        //Controller for Suggest Exhibit From Duration For User
        [HttpGet("duration-for-time")]
        public async Task<ActionResult<List<ExhibitResponse>>> SuggestExhibitFromDurationForUser([FromQuery] TimeSpan time)
        {
            
                var rs = await _durationService.SuggestExhibitFromDuration(time);
                return Ok(rs);        
        }


        //Controller for Calculate time to move and visit exhibit in event for User
        [HttpPost("total-time-to-move-and-visit-exhibit-in-a-event")]
        public async Task<ActionResult<TimeSpan>> GetTotalTimeToMoveAndVisitExhibitInEvent(int eventId, List<int> exhibitId)
        {
            var rs = await _durationService.TotalTimeForVisitorInEvent(eventId, exhibitId);
            return Ok(rs);
        }


        //Controller for Calculate time to move and visit exhibit in topic for User
        [HttpPost("total-time-to-move-and-visit-exhibit-in-a-topic")]
        public async Task<ActionResult<TimeSpan>> GetTotalTimeToMoveAndVisitExhibitInTopic(int topicId, List<int> exhibitId)
        {

            var rs = await _durationService.TotalTimeForVisitorInTopic(topicId, exhibitId);
            return Ok(rs);

        }

        //Controller for Suggest Route to move to visit exhibit for User
        [HttpPost("suggest-route-base-on-exhibit")]
        public async Task<ActionResult<List<SuggestRouteResponse>>> GetSuggestRoute(List<int> exhibitId)
        {
            var rs = await _durationService.GetRouteBaseOnExhibit(exhibitId);
            return Ok(rs);
        }

     

    }
}






