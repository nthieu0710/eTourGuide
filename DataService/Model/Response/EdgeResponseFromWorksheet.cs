using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class EdgeResponseFromWorksheet
    {
        public int Id { get; set; }

        public int FromPosition { get; set; }

        public int ToPosition { get; set; }

        public int Cost { get; set; }

        public string Description { get; set; }
    }
}
