using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTourGuide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortestPathAndSuggestRouteController : ControllerBase
    {
        private readonly IShortestPathAndSuggestRouteService _shortestPathAndSuggestRouteService;

        public ShortestPathAndSuggestRouteController(IShortestPathAndSuggestRouteService shortestPathAndSuggestRouteService)
        {
            _shortestPathAndSuggestRouteService = shortestPathAndSuggestRouteService;
        }

        [HttpGet("get/time")]
        public async Task<ActionResult<ShortestPathResponse>> GetTime([FromQuery] List<int> roomId)
        {

            var rs = await _shortestPathAndSuggestRouteService.GetShortestPath(roomId);
            return Ok(rs);

        }



        [HttpPost("suggest-route-to-back-to-startpoint")]
        public async Task<ActionResult<List<SuggestRouteResponse>>> GetSuggestRoute(int roomId)
        {
            var rs = await _shortestPathAndSuggestRouteService.GetRouteToBackToStartPoint(roomId);
            return Ok(rs);
        }
    }
}
