using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class ShortestPathResponse
    {
        public int StartNode { get; set; }
        public int Distance { get; set; }
        public List<uint> Path { get; set; }
       
    }
}
