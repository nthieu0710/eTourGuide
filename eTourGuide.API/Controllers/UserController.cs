using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.Service.Exceptions;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTourGuide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        //Controller for Search Exhibit/Event/Topic by name for User
        [HttpGet("search-by-name")]
        public async Task<ActionResult<SearchResponse>> SearchByNameForUser(string name)
        {        
            var rs = _userService.SearchByName(name);
            return Ok(rs);
         
        }


        /*[HttpPost("get-suggest-route-base-on-exhibit")]
        public ActionResult<List<SuggestRouteResponse>> GetSuggestRouteForUserBaseOnExhibit(int[] exhibitId)
        {

            var rs = _userService.GetSuggestRouteBaseOnExhibit(exhibitId);
            return Ok(rs);
        }*/

    }
}
