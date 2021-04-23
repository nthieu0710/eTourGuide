using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class PositionResponseFromWorksheet
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public decimal Dx { get; set; }

        public decimal Dy { get; set; }

        public int FloorId { get; set; }

        public int? RoomId { get; set; }

        public string DescriptionEng { get; set; }

        public string DescriptionVie { get; set; }
    }
}
