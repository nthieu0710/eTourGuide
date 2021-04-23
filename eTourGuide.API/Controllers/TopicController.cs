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
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }


        //Controller for Add Topic
        [Authorize(Roles = AccountRole.Admin)]
        [HttpPost("add/topic")]
        public async Task<ActionResult<int>> InsertTopic([FromBody] PostTopicRequest model)
        {

            var rs = await _topicService.AddTopic(model.Name, model.Description, model.NameEng, model.DescriptionEng, model.Image, model.StartDate, model.Username);          
            return Ok(rs);

        }


        //Controller for Add Exhibit to Topic for Admin
        [Authorize(Roles = AccountRole.Admin)]
        [HttpPost("add/exhibit/to/topic")]
        public async Task<ActionResult<int>> AddExhibitToTopicForAdmin([FromBody] PostExhibitInTopicRequest model)
        {

            var rs = await _topicService.AddExhibitToTopic(model.TopicId, model.ExhibitId);
            return Ok(rs);

        }


        //Controller for Update Topic
        [Authorize(Roles = AccountRole.Admin)]
        [HttpPut("id={id}")]
        public async Task<ActionResult<int>> UpdateTopic([FromBody] PutTopicRequest model, int id)
        {

            var rs = await _topicService.UpdateTopic(id, model.Name, model.Description, model.NameEng, model.DescriptionEng, model.Image, model.StartDate, model.Status);
           return Ok(rs);
           
        }


        //Controller for Active Topic
        [Authorize(Roles = AccountRole.Admin)]
        [HttpPut("active/topic/id={id}")]
        public async Task<ActionResult<int>> ActiveTopic(int id)
        {

            var rs = await _topicService.UpdateStatusFromWatingToActive(id);
            return Ok(rs);

        }

        //Controller for Delete Topic
        [Authorize(Roles = AccountRole.Admin)]
        [HttpDelete("id={id}")]
        public async Task<ActionResult<int>> DeleteTopic(int id)
        {

            var rs = await _topicService.DeleteTopic(id);
            return Ok(rs);

        }

        //Controller for Get All Topcics for Admin
        [Authorize(Roles = AccountRole.Admin)]
        [HttpGet("admin")]
        public ActionResult<List<TopicResponse>> GetAllTopcics()
        {

            var rs = _topicService.GetAllTopics();
            return Ok(rs);

        }


        //Controller for Get Topic is not set up Room
        [Authorize(Roles = AccountRole.Admin)]
        [HttpGet("get/topic/has/no/room")]
        public ActionResult<List<TopicResponse>> GetTopicThatHasNoRoom()
        {

            var rs = _topicService.GetTopicHasNoRoom();
            return Ok(rs);

        }


        //Controller for Search Topic by name for Admin
        [Authorize(Roles = AccountRole.Admin)]
        [HttpGet("search-topic-by-name-for-admin")]
        public ActionResult<List<TopicResponse>> SearchTopicByNameForAdmin(string name)
        {
         
            var rs = _topicService.SearchTopicForAdmin(name);
            return Ok(rs);

        }

        //============================================================================================//



        //Controller for Get All Topcics for User
        [HttpGet]
        public ActionResult<List<TopicResponse>> GetAllTopcicsForUser()
        {

            var rs = _topicService.GetAllTopicsForUser();
            return Ok(rs);

        }

        //Controler for Get Topic by Topic Id
        [HttpGet("id={id}")]
        public async Task<ActionResult<TopicResponse>> GetTopicById(int id)
        {

            var rs = await _topicService.GetTopicById(id);
            return Ok(rs);

        }


        //Controller for Get Highlight Topic with Rating > 4
        [HttpGet("suggest/highlight/topic")]
        public ActionResult<List<TopicResponse>> GetHighLightTopic()
        {

            var rs = _topicService.GetHightLightTopic();
            return Ok(rs);

        }

    }
}
