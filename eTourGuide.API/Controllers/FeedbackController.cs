using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.API.Models.Requests;
using eTourGuide.Data.Entity;
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
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }


        [HttpPost("add/event/feedback/from/user")]
        public async Task<ActionResult<int>> AddEventFeedbackFromUser([FromBody] PostEventFeedbackRequest model)
        {
            var rs = await _feedbackService.CreateUserFeedbackForEvent(model.EventId, model.VisitorName, model.Rating, model.Description);
            return Ok(rs);

        }


        [HttpPost("add/topic/feedback/from/user")]
        public async Task<ActionResult<int>> AddTopicFeedbackFromUser([FromBody] PostTopicFeedbackRequest model)
        {
           
            var rs = await _feedbackService.CreateUserFeedbackForTopic(model.TopicId, model.VisitorName, model.Rating, model.Description);
            return Ok(rs);
 
        }


        //Controller for Enable Feedback For Admin
        [Authorize(Roles = "1")]
        [HttpPut("enable/feeback/for/admin/id={id}")]
        public async Task<ActionResult<int>> EnableFeedback(int id)
        {
            var rs = await _feedbackService.EnableFeedbackForAdmin(id);
            return Ok(rs);

        }


        //Controller for Disable Spam Feedback For Admin
        [Authorize(Roles = "1")]
        [HttpPut("disable/feeback/for/admin/id={id}")]
        public async Task<ActionResult<int>> DisableFeedback(int id)
        {

            var rs = await _feedbackService.DisableFeedbackForAdmin(id);
            return Ok(rs);

        }


        //Controller for Get All Topic Feedback for Admin
        [Authorize(Roles = "1")]
        [HttpGet("get/feedback/event/for/admin")]
        public ActionResult<List<EventFeedbackFromUser>> GetListFeedBackEventForAdmin()
        {
            var rs = _feedbackService.GetFeedbacksEventForAdmin();
            return Ok(rs);
          
        }

        
        //Controller for Get All Topic Feedback for Admin
        [Authorize(Roles = "1")]
        [HttpGet("get/feedback/topic/for/admin")]
        public ActionResult<List<TopicFeedbackFromUser>> GetListFeedBackTopicForAdmin()
        {

            var rs = _feedbackService.GetFeedbacksTopicForAdmin();
            return Ok(rs);

        }
       

        //Controller for Get Event Feedback by Event Id
        [HttpGet("event/feedback/id={id}")]
        public ActionResult<List<EventFeedbackFromUser>> GetFeedbackForEvent(int id)
        {

            var rs = _feedbackService.GetFeedbacksEventForUserById(id);
            return Ok(rs);

        }

        //Controller for Get Topic Feedback by Topic Id
        [HttpGet("topic/feedback/id={id}")]
        public ActionResult<List<TopicFeedbackFromUser>> GetFeedbackForTopic(int id)
        {

            var rs = _feedbackService.GetFeedbacksTopicForUserById(id);
            return Ok(rs);

        }


    }
}
