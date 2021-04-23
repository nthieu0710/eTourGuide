using Dijkstra.NET.Graph;
using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IMapService
    {
        String GetMapImageUrlByFloor (int floorId);

        Task<int> ImportMapAllMapAttribute(string ImageFloor1, string ImageFloor2, List<PositionResponseFromWorksheet> PositionListFromFile, List<EdgeResponseFromWorksheet> EdgeListFromFile);
    }
}
