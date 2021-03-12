using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IFileService
    {
        //Task<bool> UploadImage(IFormFile file);

        Task<string> UploadImageToServer(string image, string targetUrl);
    }
}
