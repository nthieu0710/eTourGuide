using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using eTourGuide.Data.Entity;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IPositionService
    {
        List<PositionResponse> GetAllPositions();

        List<PositionResponse> GetPositionsForRoom(int floorId);

        List<PositionResponse> GetListPositionFromFile(List<Position> listPositions);
    }
}
