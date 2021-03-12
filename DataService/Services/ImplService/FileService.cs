using eTourGuide.Service.Services.InterfaceService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.ImplService
{
    public class FileService : IFileService
    {
        public async Task<bool> UploadImage(IFormFile file)
        {
            string username = "NguyenTrungHieu";
            string password = "NguyenTrungHieu@123!@#";

            //Create an FtpWebRequest

            var request = (FtpWebRequest)WebRequest.Create("ftp://cosplane.asia/image/" + file.FileName);
            //Set the method to UploadFile
            request.Method = WebRequestMethods.Ftp.UploadFile;
            //Set the NetworkCredentials
            request.Credentials = new NetworkCredential(username, password);

            //Set buffer length to any value you find appropriate for your use case
            byte[] buffer = new byte[1024];
            var stream = file.OpenReadStream();
            byte[] fileContents;
            //Copy everything to the 'fileContents' byte array
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                fileContents = ms.ToArray();
            }

            //Upload the 'fileContents' byte array 
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);

            }

            //Get the response
            // Some proper handling is needed
            var response = (FtpWebResponse)request.GetResponse();

            bool rs = response.StatusCode == FtpStatusCode.FileActionOK;
            return rs;
        }

        public async Task<string> UploadImageToServer(string image, string targetUrl)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create("ftp://cosplane.asia/image/" + targetUrl);
            req.UseBinary = true;
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.Credentials = new NetworkCredential("NguyenTrungHieu", "NguyenTrungHieu@123!@#");
            byte[] fileData = File.ReadAllBytes(image);
            req.ContentLength = fileData.Length;
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(fileData, 0, fileData.Length);
            reqStream.Close();

            string url = Directory.GetCurrentDirectory() + "\\" + "image" + "\\" + targetUrl;
            //Console.WriteLine(url);
            //bool rs = true;
            return url;
        }
    }
}
