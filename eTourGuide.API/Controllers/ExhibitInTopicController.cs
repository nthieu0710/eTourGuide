using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.API.Models.Requests;
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
    public class ExhibitInTopicController : ControllerBase
    {
        private readonly IExhibitInTopicService _exhibitInTopicService;

        public ExhibitInTopicController(IExhibitInTopicService exhibitInTopicService)
        {
            _exhibitInTopicService = exhibitInTopicService;
        }

        //Controller for Delete Exhibit in Topic for Admin
        [Authorize(Roles = "1")]
        [HttpDelete("delete/exhibit/in/topic/id={id}")]
        public async Task<ActionResult<int>> DeleteExhibitInTopicForAdminAsync(int id)
        {

            var rs = await _exhibitInTopicService.DeleteExhibitIntTopic(id);
            return Ok(rs);

        }




        //Controller for Get Exhibit in Topic for Admin
        [Authorize(Roles = "1")]
        [HttpGet("get/exhibit/in/topic/for/admin")]
        public ActionResult<List<ExhibitResponse>> GetExhibitInTopicForAdmin(int topicId)
        {
                
            var rs = _exhibitInTopicService.GetExhibitInTopicForAdmin(topicId);
            return Ok(rs);
              
        }



        //Controller for Get Exhibit in CLOSED Topic for Admin
        [Authorize(Roles = "1")]
        [HttpGet("get/exhibit/in/closed/topic/for/admin")]
        public ActionResult<List<ExhibitResponse>> GetExhibitInClosedTopicForAdmin(int topicId)
        {
               
            var rs = _exhibitInTopicService.GetExhbitForClosedTopic(topicId);
            return Ok(rs);
               
        }

        //=====================================================================================//

        //Controller for Get Exhibit in Topic for User
        [HttpGet("get/exhibit/in/topic/for/user")]
        public ActionResult<List<ExhibitResponse>> GetExhibitInTopic(int topicId)
        {

            var rs = _exhibitInTopicService.GetExhibitInTopic(topicId);
            return Ok(rs);
        }
    }
}
