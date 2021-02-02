using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ExhibitInEventController : ControllerBase
    {
        private readonly IExhibitInEventService _exhibitInEventService;

        public ExhibitInEventController(IExhibitInEventService exhibitInEventService)
        {
            _exhibitInEventService = exhibitInEventService;
        }

        [HttpGet]
        public ActionResult<List<ExhibitFeedbackResponse>> GetExhibitInEvent(int eventId)
        {
            try
            {
                var rs = _exhibitInEventService.GetExhibitInEvent(eventId);
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "There's not Exhibit in this Event!!!");
            }
        }
    }
}
