using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.ImplService
{
    public class MapService : IMapService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MapService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string GetMapImageUrlByFloor(int floorId)
        {
            string imageURL = "";
            Floor floor = _unitOfWork.Repository<Floor>().GetAll().Where(f => f.Id == floorId && f.Map.Status == true).FirstOrDefault(); 
            if (floor != null)
            {
                imageURL = floor.Image;
                return imageURL;
            }
            return imageURL;
        }

        public async Task<int> ImportMapAllMapAttribute(string ImageFloor1, string ImageFloor2, List<PositionResponseFromWorksheet> PositionListFromFile, List<EdgeResponseFromWorksheet> EdgeListFromFile)
        {
            int rs = 0;
            int totalPositionsForMap = 0;
            bool status = false;
            int f1 = 1;
            int f2 = 2;

            try
            {
                if (PositionListFromFile.Count() > 0)
                {
                    totalPositionsForMap = PositionListFromFile.Count();
                }

                //insert table Map
                Map map = new Map()
                {
                    TotalPositions = totalPositionsForMap,
                    Status = status
                };
                await _unitOfWork.Repository<Map>().InsertAsync(map);
                await _unitOfWork.CommitAsync();

                //insert table Floor
                //insert floor 1
                Floor floor1 = new Floor()
                {                   
                    FloorNo = f1,
                    MapId = map.Id,
                    Image = ImageFloor1
                };

                //insert floor 2
                Floor floor2 = new Floor()
                {
                    FloorNo = f2,
                    MapId = map.Id,
                    Image = ImageFloor2
                };
                await _unitOfWork.Repository<Floor>().InsertAsync(floor1);
                await _unitOfWork.Repository<Floor>().InsertAsync(floor2);
                
                await _unitOfWork.CommitAsync();

                //insert table Position
                foreach (var item in PositionListFromFile)
                {
                    int floorIdToDb = 0;

                    if (item.FloorId == 1)
                    {
                        floorIdToDb = floor1.Id;
                    }else if (item.FloorId == 2)
                    {
                        floorIdToDb = floor2.Id;
                    }

                    Position position = new Position()
                    {
                        
                        Type = item.Type,
                        Dx = (double)item.Dx,
                        Dy = (double)item.Dy,
                        FloorId = floorIdToDb,
                        RoomId = item.RoomId,
                        DescriptionEng = item.DescriptionEng,
                        DescriptionVie = item.DescriptionVie
                    };
                    await _unitOfWork.Repository<Position>().InsertAsync(position);
                    await _unitOfWork.CommitAsync();

                    if (position.RoomId != null)
                    {
                        Room room = new Room()
                        {
                            Floor = position.FloorId,
                            No = (int)position.RoomId,
                            Status = 0
                        };
                        await _unitOfWork.Repository<Room>().InsertAsync(room);
                        await _unitOfWork.CommitAsync();
                    }
                    
                }

                

                //insert table Edge
                foreach (var item in EdgeListFromFile)
                {
                    Edge edge = new Edge()
                    {
                        FromPosition = item.FromPosition,
                        ToPosition = item.ToPosition,
                        Cost = item.Cost,                       
                        Description = item.Description
                    };
                    await _unitOfWork.Repository<Edge>().InsertAsync(edge);
                    await _unitOfWork.CommitAsync();
                }       
                



                rs = 1;

                return rs;
            }
            catch (Exception e)
            {
                throw new Exception($"Some problem when import map - {e.StackTrace}");
            }
        }
    }
}
