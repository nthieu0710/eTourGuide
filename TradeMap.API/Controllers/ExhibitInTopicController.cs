using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.Service.Exceptions;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTourGuide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExhibitInTopicController : ControllerBase
    {
        private readonly IExhibitInTopicService _exhibitInTopicService;

        public ExhibitInTopicController(IExhibitInTopicService exhibitInTopicService)
        {
            _exhibitInTopicService = exhibitInTopicService;
        }

        [HttpGet]
        public ActionResult<List<ExhibitFeedbackResponse>> GetExhibitInTopic(int topicId)
        {
            try
            {
                var rs = _exhibitInTopicService.GetExhibitInTopic(topicId);
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "There's not Exhibit in this Topic!!!");
            }
        }
    }
}
