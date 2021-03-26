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
        public ActionResult<List<ExhibitResponse>> GetDurationFromExhibit([FromQuery] TimeSpan time)
        {
           
                var rs = _durationService.SuggestExhibitFromDuration(time);
                return Ok(rs);        
        }

        //Controller for Calculate time to move and visit exhibit in event for User
        [HttpPost("total-time-to-move-and-visit-exhibit-in-a-event")]
        public ActionResult<TimeSpan> GetTotalTimeToMoveAndVisitExhibitInEvent(int eventId, int[] exhibitId)
        {

            var rs = _durationService.TotalTimeForVisitorInEvent(eventId, exhibitId);
            return Ok(rs);

        }


        //Controller for Calculate time to move and visit exhibit in topic for User
        [HttpPost("total-time-to-move-and-visit-exhibit-in-a-topic")]
        public ActionResult<TimeSpan> GetTotalTimeToMoveAndVisitExhibitInTopic(int topicId, int[] exhibitId)
        {

            var rs = _durationService.TotalTimeForVisitorInTopic(topicId, exhibitId);
            return Ok(rs);

        }

        //Controller for Suggest Route to move to visit exhibit for User
        [HttpPost("suggest-route-base-on-exhibit")]
        public ActionResult<List<SuggestRouteResponse>> GetSuggestRoute(int[] exhibitId)
        {

            var rs = _durationService.GetRouteBaseOnExhibit(exhibitId);
            return Ok(rs);
        }























        /*[HttpGet("suggest-route-base-on-exhibit")]
        public ActionResult<List<int>> GetSuggestRoute([FromQuery] int[] exhibitId)
        {
          
                var rs = _durationService.SuggestRouteBaseOnExhibit(exhibitId);
                return Ok(rs);          
        }*/

        /*[HttpPost("distance-between-room")]
        public ActionResult<Double> GetDurationFromExhibit(int[] room)
        {
           
                var rs = _durationService.TotalDistance(room);
                return Ok(rs);
         
        }*/


        /* [HttpGet("total-time-for-visit-exhibit-in-event")]
         public ActionResult<TimeSpan> GetTotalTimeForExhibitInEvent(int id, [FromQuery] int[] exhibitId)
         {

                 var rs = _durationService.GetTotalTimeForVisitExhibitInEvent(id, exhibitId);
                 return Ok(rs);

         }*/


        /*[HttpGet("total-time-for-visit-exhibit-in-topic")]
        public ActionResult<TimeSpan> GetTotalTimeForExhibitInTopic(int id, [FromQuery] int[] exhibitId)
        {
        
                var rs = _durationService.GetTotalTimeForVisitExhibitInTopic(id, exhibitId);
                return Ok(rs);
            
        }*/



        /*[HttpPost("get/text/route")]
        public async Task<ActionResult<string>> GetSuggestRouteText(List<int> node)
        {

          
            var rs = _durationService.GetTextSuggestRoute(node);

            return Ok(rs);         
        }*/





    }
}






