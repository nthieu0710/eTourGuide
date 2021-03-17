using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.API.Models.Requests;
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

            [HttpGet("get/exhibit/in/topic/for/user")]
            public ActionResult<List<ExhibitResponse>> GetExhibitInTopic(int topicId)
            {
               /* try
                {*/
                    var rs = _exhibitInTopicService.GetExhibitInTopic(topicId);
                    return Ok(rs);
                /*}
                catch (Exception e)
                {
                    throw new CrudException(System.Net.HttpStatusCode.BadRequest, "There's not Exhibit in this Topic!!!");
                }*/

            }

            [HttpGet("get/exhibit/in/topic/for/admin")]
            public ActionResult<List<ExhibitResponse>> GetExhibitInTopicForAdmin(int topicId)
            {
                /*try
                {*/
                    var rs = _exhibitInTopicService.GetExhibitInTopicForAdmin(topicId);
                    return Ok(rs);
               /* }
                catch (Exception e)
                {
                    e = e.InnerException;
                    throw new CrudException(System.Net.HttpStatusCode.BadRequest, "There's not Exhibit in this Topic!!!");
                }*/
            }


            [HttpDelete("delete/exhibit/in/topic/id={id}")]
            public async Task<ActionResult<int>> DeleteExhibitInTopicForAdminAsync(int id)
            {
              /*  try
                {*/
                    var rs = await _exhibitInTopicService.DeleteExhibitIntTopic(id);
                    return Ok(rs);
              /*  }
                catch (Exception e)
                {
                   
                    throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not delete exhibit in topic!!!");
                }*/
            }
    }
}
