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
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost("add/topic/in/room")]
        public async Task<ActionResult<int>> AddTopicToRoomForAdmin([FromBody] PostTopicInRoomRequest model)
        {
           /* try
            {*/
                var rs = await _roomService.AddTopicToRoom(model.TopicId, model.RoomId);
                return Ok(rs);
           /* }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Add Topic to Room Error!!!");
            }*/
        }

        [HttpPost("add/event/in/room")]
        public async Task<ActionResult<int>> AddEventToRoomForAdmin([FromBody] PostEvemtInRoomRequest model)
        {
           /* try
            {*/
                var rs = await _roomService.AddEventToRoom(model.EventId, model.RoomId);
                return Ok(rs);
            /*}
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Add Event to Room Error!!!");
            }*/
        }


        [HttpGet("get/room/from/exhibit")]
        public ActionResult<List<RoomResponse>> GetRoom([FromQuery] int[] exhibitId)
        {
          /*  try
            {*/
                var rs = _roomService.GetRoomForExhibit(exhibitId); 
                return Ok(rs);
           /* }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Room Error!!!");
            }*/
        }


        [HttpGet("get/exhibit/from/room")]
        public ActionResult<List<RoomResponse>> GetExhibit(int roomId)
        {
           /* try
            {*/
                var rs = _roomService.GetExhibitFromRoom(roomId);
                return Ok(rs);
            /*}
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Exhibit Error!!!");
            }*/
        }



        [HttpGet("get/all/room")]
        public ActionResult<List<RoomResponse>> GetAllRooms()
        {
           /* try
            {*/
                var rs = _roomService.GetAllRoom();
                return Ok(rs);
            /*}
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Get Exhibit Error!!!");
            }*/
        }


        
        [HttpGet("get/event/or/topic/in/room")]
        public ActionResult<ObjectResponseInRoomForAdmin> GetEventOrTopicInRoom(int roomId)
        {
           /* try
            {*/
                var rs = _roomService.GetTopicOrEventInRoom(roomId);
               /* if (rs == null)
                {
                    return null;
                }*/
                return Ok(rs);
           /* }
            catch (Exception e)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "There's no Topcic or Event in this Room!!!");
            }*/
        }


        [HttpDelete("delete/topic/in/room/id={id}")]
        public async Task<ActionResult<int>> DeleteTopicInRoomForAdminAsync(int id)
        {
          /*  try
            {*/
                var rs = await _roomService.DeleteTopicInRoom(id);
                return Ok(rs);
           /* }
            catch (Exception)
            {

                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not delete topic in room!!!");
            }*/
        }


        [HttpDelete("delete/event/in/room/id={id}")]
        public async Task<ActionResult<int>> DeleteEventInRoomForAdminAsync(int id)
        {
          /*  try
            {*/
                var rs = await _roomService.DeleteEventInRoom(id);
                return Ok(rs);
           /* }
            catch (Exception)
            {

                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not delete topic in room!!!");
            }*/
        }
    }
}
