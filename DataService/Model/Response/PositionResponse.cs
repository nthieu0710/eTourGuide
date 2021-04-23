using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class PositionResponse
    {
        public int Id { get; set; }
        public string DescriptionEng { get; set; }
        public string DescriptionVie { get; set; }
        public int Type { get; set; }
        public double Dx { get; set; }
        public double Dy { get; set; }
        public int FloorId { get; set; }
        public int? RoomId { get; set; }

    }
}
