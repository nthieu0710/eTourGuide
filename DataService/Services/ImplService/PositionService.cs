using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using eTourGuide.Service.Helpers;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.ImplService
{
    public class PositionService : IPositionService
    {

        private readonly IUnitOfWork _unitOfWork;
        
        public PositionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;          
        }

        public List<PositionResponse> GetAllPositions()
        {
            List<PositionResponse> listResponse = new List<PositionResponse>();

            var listPosition = _unitOfWork.Repository<Position>().GetAll()
                                        .Where(p => p.Floor.Map.Status == true).ToList();
            if (listPosition.Count() > 0)
            {
                foreach(var item in listPosition)
                {
                    PositionResponse response = new PositionResponse()
                    {
                        Dx = (double)item.Dx,
                        Dy = (double)item.Dy
                    };
                    listResponse.Add(response);
                }
            }
            return listResponse.ToList();
        }


        public List<PositionResponse> GetPositionsForRoom(int floorId)
        {
            
            List<PositionResponse> listResponse = new List<PositionResponse>();
          
            var listPosition = _unitOfWork.Repository<Position>().GetAll()
                                        .Where(p => p.FloorId == floorId
                                                    && p.FloorId == p.Floor.Id
                                                    && p.Floor.Map.Status == true
                                                    && (p.Type == 2 || p.Type == 3 || p.Type == 4)).ToList();

            if (listPosition.Count() > 0)
            {
                foreach (var item in listPosition)
                {
                    int roomIdResponse = 0;
                    if (item.RoomId == null)
                    {
                        roomIdResponse = 0;
                    }
                    else
                    {
                        roomIdResponse = (int)item.RoomId;
                    }
                    PositionResponse response = new PositionResponse()
                    {
                        Dx = (double)item.Dx,
                        Dy = (double)item.Dy,
                        FloorId = (int)item.FloorId,
                        RoomId = roomIdResponse,
                        Type = (int)item.Type
                    };
                    listResponse.Add(response);
                }
            }

                return listResponse.ToList();
        }

        public List<PositionResponse> GetListPositionFromFile(List<Position> listPositions)
        {
            List<PositionResponse> listPositionResponses = new List<PositionResponse>();        

            if (listPositions.Count() > 0)
            {
                foreach(var item in listPositions)
                {
                    PositionResponse response = new PositionResponse()
                    {
                        Id = item.Id,
                        DescriptionEng = item.DescriptionEng, 
                        DescriptionVie = item.DescriptionVie,
                        Type = (int)item.Type,
                        Dx = (double)item.Dx,
                        Dy = (double)item.Dy,
                        FloorId = (int)item.FloorId,
                        RoomId = (int)item.RoomId
                    };
                    listPositionResponses.Add(response);
                }

            }

            return listPositionResponses.ToList();
        }

    }
}
