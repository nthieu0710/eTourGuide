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
    public class ExhibitController : ControllerBase
    {
        private readonly IExhibitService _exhibitService;

        public ExhibitController(IExhibitService exhibitService)
        {
            _exhibitService = exhibitService;
        }



        //Controller for Add Exhibit
        [Authorize(Roles = "1")]
        [HttpPost("add/exhibit")]
        public async Task<ActionResult<int>> InsertExhibit([FromBody] PostExhibitRequest model)
        {
           
            var rs = await _exhibitService.AddExhibit(model.Name, model.Description, model.NameEng, model.DescriptionEng, model.Image, model.Duration);
            return Ok(rs);
           
        }
        
        
        //Controller for Update Exhibit
        [Authorize(Roles = "1")]
        [HttpPut("id={id}")]
        public async Task<ActionResult<int>> UpdateExhibit([FromBody] PutExhibitRequest model, int id)
        {
           
            var rs = await _exhibitService.UpdateExhibit(id, model.Name, model.Description, model.NameEng, model.DescriptionEng, model.Image, model.Duration);
            return Ok(rs);
          
        }


        //Controller for Delete Event
        [Authorize(Roles = "1")]
        [HttpDelete("id={id}")]
        public async Task<ActionResult<int>> DeleteExhibit(int id)
        {
        
            var rs = await _exhibitService.DeleteExhibit(id);
            return Ok(rs);
         
        }


        //Controller for Delete Event
        [Authorize(Roles = "1")]
        [HttpGet("get-topic-or-event-contain-exhibit")]
        public  ActionResult<String> GetTopicOrEventContainExhibitForAdmin([FromQuery] int exhibitId)
        {

            var rs =  _exhibitService.GetTopicOrEventContainExhibit(exhibitId);
            return Ok(rs);

        }


        //Controller for Get All Exhibit for Admin
        [Authorize(Roles = "1")]
        [HttpGet("get/all/exhibit/for/admin")]
        public ActionResult<List<ExhibitResponse>> GetAllExhibitsForAdmin()
        {        
            var rs = _exhibitService.GetAllExhibitForAdmin();
            return Ok(rs);
          
        }


        //Controller for get Exhibit is not in any topic or event for Admin
        [Authorize(Roles = "1")]
        [HttpGet("get/available/exhibit")]
        public ActionResult<List<ExhibitResponse>> GetAvailableExhibit()
        {
           
            var rs = _exhibitService.GetAvailableExhibit(); ;
            return Ok(rs);
           
        }


        //Controller for Search by name exhibit for Admin
        [Authorize(Roles = "1")]
        [HttpGet("search-exhibit-by-name-for-admin")]
        public ActionResult<List<ExhibitResponse>> SearchExhibitByNameForAdmin(string name)
        {

            var rs = _exhibitService.SearchExhibitForAdmin(name);
            return Ok(rs);

        }


        //===========================================================================//

        //Get Highlight Exhibit with Rating > 4 for User
        [HttpGet("suggest/highlight/exhibit")]
        public ActionResult<List<ExhibitResponse>> GetHighLightObject()
        {
           
            var rs = _exhibitService.GetHightLightExhibit(); 
            return Ok(rs);
          
        }


        //Controller for Get All Exhibit for User
        [HttpGet]
        public ActionResult<List<ExhibitResponse>> GetAllTopcicsForUser()
        {        
            var rs = _exhibitService.GetAllExhibitsForUser();
            return Ok(rs);         
        }


        //Controller for Get Exhibit was created nearly for User
        [HttpGet("recently/createdate/exhibit")]
        public ActionResult<List<ExhibitResponse>> GetRecentlyExhibit()
        {
          
            var rs = _exhibitService.GetNewExhibit(); ;
            return Ok(rs);
           
        }





        

    }
}
