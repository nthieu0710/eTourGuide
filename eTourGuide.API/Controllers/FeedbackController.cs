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
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("exhibit/feedback/id={id}")]
        public ActionResult<List<Feedback>> GetFeedbackForExhibit(int id)
        {
            try
            {
                var rs = _feedbackService.GetFeedbacksExhibitcForUserById(id);
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Feedback Exhibit Error!!!");
            }
        }


        [HttpGet("event/feedback/id={id}")]
        public ActionResult<List<Feedback>> GetFeedbackForEvent(int id)
        {
            try
            {
                var rs = _feedbackService.GetFeedbacksEventForUserById(id);
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Feedback Event Error!!!");
            }
        }

        [HttpGet("topic/feedback/id={id}")]
        public ActionResult<List<Feedback>> GetFeedbackForTopic(int id)
        {
            try
            {
                var rs = _feedbackService.GetFeedbacksTopicForUserById(id);
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Feedback Topic Error!!!");
            }
        }



        [HttpGet("get/feedback/event/for/admin")]
        public ActionResult<List<EventFeedbackFromUser>> GetListFeedBackEventForAdmin()
        {
            try
            {
                var rs = _feedbackService.GetFeedbacksEventForAdmin();
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Feedback Event Error!!!");
            }
        }


        [HttpGet("get/feedback/exhibit/for/admin")]
        public ActionResult<List<ExhibitFeedbackFromUser>> GetListFeedBackExhibitForAdmin()
        {
            try
            {
                var rs = _feedbackService.GetFeedbacksExhibitForAdmin();
                return Ok(rs);
            }
            catch (Exception e)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Feedback Exhibit Error!!!");
            }
        }


        [HttpGet("get/feedback/topic/for/admin")]
        public ActionResult<List<TopicFeedbackFromUser>> GetListFeedBackTopicForAdmin()
        {
            try
            {
                var rs = _feedbackService.GetFeedbacksTopicForAdmin();
                return Ok(rs);
            }
            catch (Exception e)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Feedback Topic Error!!!");
            }
        }



        
        [HttpPost("add/exhibit/feedback/from/user")]
        public async Task<ActionResult<int>> AddExhibitFeedbackFromUser([FromBody] PostExhibitFeedbackRequest model)
        {

            try
            {
                var rs = await _feedbackService.CreateUserFeedbackForExhibit(model.ExhibitId, model.VisitorName, model.Rating, model.Description);

                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest("Can Not Create Feedback!");
            }

        }


        [HttpPost("add/event/feedback/from/user")]
        public async Task<ActionResult<int>> AddEventFeedbackFromUser([FromBody] PostEventFeedbackRequest model)
        {

            try
            {
                var rs = await _feedbackService.CreateUserFeedbackForEvent(model.EventId, model.VisitorName, model.Rating, model.Description);

                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest("Can Not Create Feedback!");
            }

        }


        [HttpPost("add/topic/feedback/from/user")]
        public async Task<ActionResult<int>> AddTopicFeedbackFromUser([FromBody] PostTopicFeedbackRequest model)
        {

            try
            {
                var rs = await _feedbackService.CreateUserFeedbackForTopic(model.TopicId, model.VisitorName, model.Rating, model.Description);

                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest("Can Not Create Feedback!");
            }

        }

    }
}
