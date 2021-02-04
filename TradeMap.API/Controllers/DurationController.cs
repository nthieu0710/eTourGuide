using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.API.Model.Response;
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



        [HttpGet("duration/exhibit")]
        public ActionResult<List<ExhibitResponseForUser>> GetDurationFromExhibit([FromQuery] GetDurationRequest model)
        {
            try
            {
                var rs = _durationService.SuggestExhibitFromDuration(model.Time);
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Duration Error!!!");
            }
        }


        [HttpGet("totalTimeForExhibitInEvent")]
        public ActionResult<TimeSpan> GetTotalTimeForExhibitInEvent(int id, [FromQuery] int[] exhibitId)
        {
            try
            {
                var rs = _durationService.GetTotalTimeForVisitExhibitInEvent(id, exhibitId);
                return Ok(rs);
            }
            catch (Exception e)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Total Error!!!");
            }
        }


        [HttpGet("totalTimeForExhibitInTopic")]
        public ActionResult<TimeSpan> GetTotalTimeForExhibitInTopic(int id, [FromQuery] int[] exhibitId)
        {
            try
            {
                var rs = _durationService.GetTotalTimeForVisitExhibitInTopic(id, exhibitId);
                return Ok(rs);
            }
            catch (Exception e)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Total Error!!!");
            }
        }
    }
}






/*[HttpGet("duration/event")]
public ActionResult<List<ExhibitResponseForUser>> GetDurationForEvent([FromQuery] GetDurationRequest model)
{
    try
    {
        var rs = _durationService.DurationForEvent(model.Id, model.Time);
        return Ok(rs);
    }
    catch (Exception)
    {
        throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Duration Event Error!!!");
    }
}


[HttpGet("duration/topic")]
public ActionResult<List<ExhibitResponseForUser>> GetDurationForTopic([FromQuery] GetDurationRequest model)
{
    try
    {
        var rs = _durationService.DurationForTopic(model.Id, model.Time);
        return Ok(rs);
    }
    catch (Exception)
    {
        throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Duration Topic Error!!!");
    }
}
    }
}
*/