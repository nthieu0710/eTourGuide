using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class VerifyResponse
    {
        public string Jwt { get; set; }
        public string RefreshToken { get; set; }
    }
}
