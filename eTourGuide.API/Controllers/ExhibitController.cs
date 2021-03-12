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



        //Controller for Insert Exhibit
        [HttpPost("add/exhibit")]
        public async Task<ActionResult<int>> InsertExhibit([FromBody] PostExhibitRequest model)
        {
            try
            {
                var rs = await _exhibitService.AddExhibit(model.Name, model.Description, model.Image, model.Duration);
                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest("Can Not Insert Exhibit!");
            }
        }

        //Controller for Update Exhibit
        [HttpPut("id={id}")]
        public async Task<ActionResult<int>> UpdateExhibit([FromBody] PutExhibitRequest model, int id)
        {
            try
            {
                var rs = await _exhibitService.UpdateExhibit(id, model.Name, model.Description, model.Image, model.Duration);
                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest("Can Not Update Exhibit!");
            }
        }


        [HttpDelete("id={id}")]
        public async Task<ActionResult<int>> DeleteExhibit(int id)
        {

            try
            {
                var rs = await _exhibitService.DeleteExhibit(id);
                return Ok(rs);
            }
            catch (Exception)
            {
                return BadRequest("Can Not Delete Exhibit!");
            }
        }


        //Controller for GetAllTopcics for Admin
        [HttpGet("get/all/exhibit/for/admin")]
        public ActionResult<List<ExhibitResponse>> GetAllExhibitsForAdmin()
        {
            try
            {
                var rs = _exhibitService.GetAllExhibitForAdmin();
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Exhibits Error!!!");
            }
        }



        [HttpGet("get/available/exhibit")]
        public ActionResult<List<ExhibitFeedbackResponse>> GetAvailableExhibit()
        {
            try
            {
                var rs = _exhibitService.GetAvailableExhibit(); ;
                return Ok(rs);
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Highlight Exhibit Error!!!");
            }
        }


        //Get Highlight Object Rating > 4
        [HttpGet("suggest/highlight/exhibit")]
        public ActionResult<List<ExhibitFeedbackResponse>> GetHighLightObject()
        {
            try
            {
                var rs = _exhibitService.GetHightLightExhibit(); ;
                return Ok(rs);
            }
            catch (Exception e)
            {
                e = e.InnerException;
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
