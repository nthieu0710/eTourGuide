using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using eTourGuide.API.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Servcies.InterfaceService;

namespace TradeMap.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        //Controller for First Login Admin
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> AuthenticateWebAdminAsync([FromBody] AuthenticateModelWebAdmin model)
        {
            var authenticateResponse = await _accountService.AuthenticateAsync(model.username, model.password);

            if (authenticateResponse == null)
                return null;
            return Ok(authenticateResponse);
        }


        //Controller for verify next access
        /// <summary>
        /// Verify jwt token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /*[AllowAnonymous]
        [HttpPost("verify-jwt")]
        public ActionResult<VerifyResponse> VerifyJwt([FromBody] VerifyJwtRequest model)
        {
            var rs = _accountService.VerifyJwtLogin(model.Jwt);
            return Ok(rs);
        }*/

    }
}
