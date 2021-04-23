using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.API.Models.Requests;
using eTourGuide.Data.Entity;
using eTourGuide.Service.Helpers;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace eTourGuide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService postitionService)
        {
            _positionService = postitionService;
        }

        //get tọa độ của map đang hiện hành
        [HttpGet("get-all-positions-for-app")]
        public ActionResult<List<PositionResponse>> GetAllPositionsForApp()
        {
            var rs = _positionService.GetAllPositions();
            return Ok(rs);

        }


        //get tọa độ của map đang hiện hành
        [HttpGet("get-positions-for-room")]
        public ActionResult<List<PositionResponse>> GetPositionForRoomApp(int floorId)
        {
            var rs = _positionService.GetPositionsForRoom(floorId);
            return Ok(rs);

        }

    }
}
