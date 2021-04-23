using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTourGuide.API.Models.Requests
{
    public class PostMapRequest
    {
        /// <summary>
        /// Image url for floor 1
        /// </summary>
        public string Floor1 { get; set; }

        /// <summary>
        /// Image url for floor 2
        /// </summary>
        public string Floor2 { get; set; }

        /// <summary>
        /// Excel file for positions
        /// </summary>
        public IFormFile PositionsFile { get; set; }

        /// <summary>
        /// Excel file for edges
        /// </summary>
        public IFormFile EdgesFile { get; set; }


       
    }
}
