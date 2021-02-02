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
    public class ExhibitController : ControllerBase
    {
        private readonly IExhibitService _exhibitService;

        public ExhibitController(IExhibitService exhibitService)
        {
            _exhibitService = exhibitService;
        }


        //Controller for InsertExhibit
        /*[HttpPost]
        public async Task<ActionResult<Exhibit>> InsertExhibit([FromBody] PostExhibitRequest model)
        {

            try
            {
                var rs = await _exhibitService.AddExhibit(model.Name, model.Description, model.Image, model.Rating, model.Status);
                return Ok(rs);
            }
            catch (Exception)
            {

                return BadRequest("Can Not Insert Exhibit!");
            }

        }*/


        //Get Highlight Object Rating > 4
        [HttpGet("suggest/highlight/exhibit")]
        public ActionResult<List<ExhibitFeedbackResponse>> GetHighLightObject()
        {
            try
            {
                var rs = _exhibitService.GetHightLightExhibit(); ;
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Highlight Exhibit Error!!!");
            }
        }


        //Controller for GetAllTopcics for User
        [HttpGet]
        public ActionResult<List<ExhibitResponseForUser>> GetAllTopcicsForUser()
        {
            try
            {
                var rs = _exhibitService.GetAllExhibitsForUser();
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Exhibit Error!!!");
            }
        }



        [HttpGet("recently/createdate/exhibit")]
        public ActionResult<List<ExhibitResponseForUser>> GetRecentlyExhibit()
        {
            try
            {
                var rs = _exhibitService.GetNewExhibit(); ;
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Highlight Exhibit Error!!!");
            }
        }

    }
}
