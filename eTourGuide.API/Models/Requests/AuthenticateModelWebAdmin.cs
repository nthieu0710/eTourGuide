using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eTourGuide.API.Models.Requests
{
    public class AuthenticateModelWebAdmin
    {
        //[Required]
        public string username { get; set; }
        public string password { get; set; }
    }
}
