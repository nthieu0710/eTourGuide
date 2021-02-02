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


        
    }
}
