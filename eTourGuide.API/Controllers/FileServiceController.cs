using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.API.Models.Requests;
using eTourGuide.Service.Services.InterfaceService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTourGuide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileServiceController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileServiceController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /*[HttpPost("upload/image")]
        public async Task<ActionResult<bool>> UploadImageToServer(IFormFile File)
        {

            try
            {
                var rs = await _fileService.UploadImage(File);

                return Ok(rs);
            }
            catch (Exception e)
            {
                return BadRequest("Can Not Upload Image!");
            }

        }*/


        [HttpPost("upload/image")]
        public async Task<ActionResult<string>> UploadImageToServer(string image, string targetUrl)
        {

            try
            {
                var rs = await _fileService.UploadImageToServer(image, targetUrl);

                return Ok(rs);
            }
            catch (Exception e)
            {
                return BadRequest("Can Not Upload Image!");
            }

        }


    }
}
