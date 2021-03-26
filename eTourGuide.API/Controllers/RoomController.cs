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
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        //Controller for Set Room for Topic
        [Authorize(Roles = "1")]
        [HttpPost("add/topic/in/room")]
        public async Task<ActionResult<int>> AddTopicToRoomForAdmin([FromBody] PostTopicInRoomRequest model)
        {
         
            var rs = await _roomService.AddTopicToRoom(model.TopicId, model.RoomId);
            return Ok(rs);

        }

        //Controller for Set Room for Event
        [Authorize(Roles = "1")]
        [HttpPost("add/event/in/room")]
        public async Task<ActionResult<int>> AddEventToRoomForAdmin([FromBody] PostEvemtInRoomRequest model)
        {

            var rs = await _roomService.AddEventToRoom(model.EventId, model.RoomId);
            return Ok(rs);

        }


        //Controller for Delete Topic in Room
        [Authorize(Roles = "1")]
        [HttpDelete("delete/topic/in/room/id={id}")]
        public async Task<ActionResult<int>> DeleteTopicInRoomForAdminAsync(int id)
        {

            var rs = await _roomService.DeleteTopicInRoom(id);
            return Ok(rs);

        }

        //Controller for Delete Event in Room
        [Authorize(Roles = "1")]
        [HttpDelete("delete/event/in/room/id={id}")]
        public async Task<ActionResult<int>> DeleteEventInRoomForAdminAsync(int id)
        {

            var rs = await _roomService.DeleteEventInRoom(id);
            return Ok(rs);

        }




        //Controller for Get Event or Topic in Room
        [Authorize(Roles = "1")]
        [HttpGet("get/event/or/topic/in/room")]
        public ActionResult<ObjectResponseInRoomForAdmin> GetEventOrTopicInRoom(int roomId)
        {

            var rs = _roomService.GetTopicOrEventInRoom(roomId);
            return Ok(rs);

        }

       


        //=============================================================//


        //Controller for Get Room from Exhibit was chosen by User
        [HttpGet("get/room/from/exhibit")]
        public ActionResult<List<RoomResponse>> GetRoom([FromQuery] int[] exhibitId)
        {
            
            var rs = _roomService.GetRoomForExhibit(exhibitId);
            return Ok(rs);

        }


        //Controller for Get Exhibit from Room was chosen by User
        [HttpGet("get/exhibit/from/room")]
        public ActionResult<List<RoomResponse>> GetExhibit(int roomId)
        {

            var rs = _roomService.GetExhibitFromRoom(roomId);
            return Ok(rs);
 
        }


        //Controller for Get Room was having Topic or Event
        [HttpGet("get/all/room")]
        public ActionResult<List<RoomResponse>> GetAllRooms()
        {
            var rs = _roomService.GetAllRoom();
            return Ok(rs);
            
        }

    }
}
