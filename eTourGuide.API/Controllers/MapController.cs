using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.Service.Services.InterfaceService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using eTourGuide.API.Models.Requests;
using OfficeOpenXml;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Helpers;

namespace eTourGuide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IMapService _mapService;

        public MapController(IMapService mapService)
        {
            _mapService = mapService;
        }


        //Controller for Get Image Admin
        [Authorize(Roles = AccountRole.Admin)]
        [HttpGet("get-map-image-by-floor-for-admin")]
        public ActionResult<String> GetImageUrlForAdmin([FromQuery] int FloorId)
        {
            var rs = _mapService.GetMapImageUrlByFloor(FloorId);
            return Ok(rs);
        }

        [HttpGet("get-map-image-by-floor-for-user")]
        public ActionResult<String> GetImageUrlForUser([FromQuery] int FloorId)
        {
            var rs = _mapService.GetMapImageUrlByFloor(FloorId);
            return Ok(rs);
        }

        [Authorize(Roles = AccountRole.Admin)]
        [HttpPost("import-map-for-admin")]
        public async Task<ActionResult<int>> ImportMapAllMapAttributeAdmin([FromForm] PostMapRequest request)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var packagePosition = new ExcelPackage(request.PositionsFile.OpenReadStream());

            using var packageEdge = new ExcelPackage(request.EdgesFile.OpenReadStream());

            

            // Get first worksheet Position
            var worksheetPosition = packagePosition.Workbook.Worksheets[0];

            var position = worksheetPosition.ToObject<PositionResponseFromWorksheet>();

            
            // Get first worksheet Edge
            var worksheetEdge = packageEdge.Workbook.Worksheets[0];

            var edge = worksheetEdge.ToObject<EdgeResponseFromWorksheet>();
                    

            //Import Map
            var rs = await  _mapService.ImportMapAllMapAttribute(request.Floor1, request.Floor2, position, edge);
            return Ok(rs);
        }

    }
}
