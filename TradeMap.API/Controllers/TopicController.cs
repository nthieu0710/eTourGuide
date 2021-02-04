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
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        //Controller for GetAllTopcics for Admin
        [HttpGet("admin")]
        public ActionResult<List<TopicResponse>> GetAllTopcics()
        {
            try
            {
                var rs = _topicService.GetAllTopics();
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Topics Error!!!");
            }
        }

        //Controller for GetAllTopcics for User
        [HttpGet]
        public ActionResult<List<TopicResponseForUser>> GetAllTopcicsForUser()
        {
            try
            {
                var rs = _topicService.GetAllTopicsForUser();
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Topics Error!!!");
            }
        }

        //Controlerr for Get by Id
        [HttpGet("id={id}")]
        public async Task<ActionResult<TopicResponse>> GetTopicById(int id)
        {
            try
            {
                var rs = _topicService.GetTopicById(id);
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not find Topic!!!");
            }
        }


        //Get Highlight Topic Rating > 4
        [HttpGet("suggest/highlight/topic")]
        public ActionResult<List<TopicFeedbackResponse>> GetHighLightTopic()
        {
            try
            {
                var rs = _topicService.GetHightLightTopic();
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Highlight Topic Error!!!");
            }
        }


        //Controller for InsertTopic
        [HttpPost]
        public async Task<ActionResult<Topic>> InsertTopic([FromBody] PostTopicRequest model)
        {

            try
            {
                var rs = await _topicService.AddTopic(model.Name, model.Description, model.Image, model.StartDate);
                
                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest("Can Not Insert Topic!");
            }

        }



        //Controller for UpdateTopic
        [HttpPut("id={id}")]
        public async Task<ActionResult<Topic>> UpdateTopic([FromBody] PutTopicRequest model, int id)
        {

            try
            {
                var rs = await _topicService.UpdateTopic(id, model.Name, model.Description, model.Image, model.StartDate, model.Status);
                return Ok(rs);
            }
            catch (Exception)
            {

                return BadRequest("Can Not Update Topic!");
            }
        }

        [HttpPut("active/topic/id={id}")]
        public async Task<ActionResult<int>> ActiveTopic(int id)
        {

            try
            {
                var rs = await _topicService.UpdateStatusFromWatingToActive(id);
                return Ok(rs);
            }
            catch (Exception)
            {

                return BadRequest("Can Not Update Topic!");
            }
        }



        [HttpDelete("id={id}")]
        public async Task<ActionResult<Topic>> DeleteTopic(int id)
        {

            try
            {
                var rs = await _topicService.DeleteTopic(id);
                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest("Can Not Delete Topic!");
            }
        }

        
    }
}
