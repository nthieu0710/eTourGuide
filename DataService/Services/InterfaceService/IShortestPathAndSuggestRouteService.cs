using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IShortestPathAndSuggestRouteService
    {

        Task<ShortestPathResponse> GetShortestPath(List<int> roomId);

        Task<TimeSpan> GetTimeToMoveFromRoom(List<int> roomId);

        Task<List<SuggestRouteResponse>> GetRouteBaseOnExhibitForUser(List<int> exhibitId);

        Task<List<SuggestRouteResponse>> GetRouteToBackToStartPoint(int roomId);
    }
}
