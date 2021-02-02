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


        [HttpGet("duration/event")]
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
