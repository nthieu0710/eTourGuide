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
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }


        [HttpGet("roomNe")]
        public ActionResult<List<RoomResponse>> GetRoom([FromQuery] int[] exhibitId)
        {
            try
            {
                var rs = _roomService.GetRoomForExhibit(exhibitId); 
                return Ok(rs);
            }
            catch (Exception e)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Room Error!!!");
            }
        }
    }
}
