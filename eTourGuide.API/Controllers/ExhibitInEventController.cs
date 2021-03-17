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
    public class ExhibitInEventController : ControllerBase
    {
        private readonly IExhibitInEventService _exhibitInEventService;

        public ExhibitInEventController(IExhibitInEventService exhibitInEventService)
        {
            _exhibitInEventService = exhibitInEventService;
        }

        [HttpGet("get/exhibit/in/event/for/user")]
        public ActionResult<List<ExhibitResponse>> GetExhibitInEvent(int eventId)
        {
            /*try
            {*/
                var rs = _exhibitInEventService.GetExhibitInEvent(eventId);
                return Ok(rs);
            /*}
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "There's not Exhibit in this Event!!!");
            }*/
        }


        [HttpDelete("delete/exhibit/in/event/id={id}")]
        public async Task<ActionResult<int>> DeleteExhibitInEventForAdminAsync(int id)
        {
           /* try
            {*/
                var rs = await _exhibitInEventService.DeleteExhibitInEvent(id);
                return Ok(rs);
            /*}
            catch (Exception e)
            {

                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not delete exhibit in event!!!");
            }*/
        }


        [HttpGet("get/exhibit/in/event/for/admin")]
        public ActionResult<List<ExhibitResponse>> GetExhibitInEventForAdmin(int eventId)
        {
           /* try
            {*/
                var rs = _exhibitInEventService.GetExhibitInEventForAdmin(eventId);
                return Ok(rs);
           /* }
            catch (Exception e)
            {
                e = e.InnerException;
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "There's not Exhibit in this Event!!!");
            }*/
        }
    }
}
