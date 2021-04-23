using Dijkstra.NET.Graph;
using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dijkstra.NET.ShortestPath;
using System.Text;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using eTourGuide.Service.Helpers;

namespace eTourGuide.Service.Services.ImplService
{
    public class ShortestPathAndSuggestRouteService : IShortestPathAndSuggestRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomService _roomService;

        public ShortestPathAndSuggestRouteService(IUnitOfWork unitOfWork, IRoomService roomService)
        {
            _unitOfWork = unitOfWork;
            _roomService = roomService;
        }


        public Graph<int, string> CreateGraph()
        {
            //bổ sung diễn đạt kiểu
            Graph<int, string> graph = new Graph<int, string>();
            //List<Data.Entity.Edge> listEdgeResponse = new List<Data.Entity.Edge>();

           Map map = _unitOfWork.Repository<Map>().GetAll().Where(m => m.Status == true).FirstOrDefault();
            
            if (map != null)
            {
                var listPositionFromMap = _unitOfWork.Repository<Position>().GetAll()
                                        .Where(p => p.FloorId == p.Floor.Id && p.Floor.MapId == map.Id).ToList();

                /*var listPositionFromMap = _unitOfWork.Repository<Position>().GetAll().
                                            Where(p => p.Floor.Map.Id == map.Id && p.Floor.Map.Status == true).ToList();*/

                if (listPositionFromMap.Count() > 0)
                {
                    foreach(var item in listPositionFromMap)
                    {
                        graph.AddNode(item.Id);
                    }
                }

                var edgeList = _unitOfWork.Repository<Data.Entity.Edge>().GetAll().
                                Where(e => e.ToPosition == e.ToPositionNavigation.Id
                                            && e.FromPosition == e.FromPositionNavigation.Id
                                            && e.ToPositionNavigation.FloorId == e.ToPositionNavigation.Floor.Id
                                            && e.ToPositionNavigation.Floor.MapId == e.ToPositionNavigation.Floor.Map.Id
                                            && e.ToPositionNavigation.Floor.Map.Status == true).ToList();
                
                if (edgeList.Count() > 0)
                {
                    for (int i = 0; i < edgeList.Count(); i++)
                    {
                        graph.Connect((uint)edgeList[i].FromPosition, (uint)edgeList[i].ToPosition, edgeList[i].Cost, edgeList[i].Description);
                                  
                    } 
                }
            }
            return graph;
        }

        public async Task<ShortestPathResponse> GetShortestPath(List<int> roomId)
        {

            Graph<int, string> graph = CreateGraph();


            Dictionary<uint, bool> arriveList = new Dictionary<uint, bool>();


            //lấy thằng điểm bắt đầu của map đang hiện hành
            Position startPosition =  await _unitOfWork.Repository<Position>().GetAll().
                                    Where(p => p.DescriptionEng == "Start point"
                                            && p.FloorId == p.Floor.Id 
                                            && p.Floor.MapId == p.Floor.Map.Id 
                                            && p.Floor.Map.Status == true).FirstOrDefaultAsync();  
            
            if (startPosition != null)
            {
                arriveList.Add((uint)startPosition.Id, false);
            }

            //arriveList.Add(1, false);


            //duyệt trong list room đc nhập vào để quét table Position trong databse lấy ra node tương ứng với room, điêu kiện roomid bằng vs id truyền vào và type phải là true (true == node)
            List<int> inputPositions = new List<int>();

            if (roomId.Count() == 1)
            {
                var position = _unitOfWork.Repository<Position>().GetAll().Where(p => p.RoomId == roomId.ElementAt(0)).FirstOrDefault();
                if (position != null)
                {
                    inputPositions.Add((int)position.Id);
                }
            }

            else
            {
                foreach (int id in roomId)
                {
                    var position = _unitOfWork.Repository<Position>().GetAll().Where(p => p.RoomId == id).FirstOrDefault();
                    if (position != null)
                    {
                        inputPositions.Add((int)position.Id);
                    }
                }
            }



            foreach (int node in inputPositions)
            {
                arriveList.Add((uint)node, false);
            }


            ShortestPathResponse shortestPath = new ShortestPathResponse()
            {
                StartNode = 0,
                Distance = int.MaxValue,
                Path = new List<uint>()
            };

            uint startNode;
            //vòng for đầu tiên để đổi các start node theo vòng
            foreach (var node in arriveList.Keys.ToList())
            {
                Debug.WriteLine("Start from node {0}:", node);
                startNode = node;
                List<uint> result = new List<uint> { node };

                foreach (var key in arriveList.Keys.ToList())
                {
                    arriveList[key] = false;
                }
                //lúc hết 1 vòng for thì status list auto = false hết các phần tử, trừ start node
                arriveList[startNode] = true;

                int totalDistance = 0;

                // Lặp cho đến khi không còn must visit node nào chưa đi qua
                while (arriveList.Values.Any(x => x == false))
                {
                    //chỗ này để t tính total distance của 1 node bắt đầu để so sánh
                    //int count = 0;
                    int minDistance = int.MaxValue;
                    uint nextNode = 0;
                    ShortestPathResult pathResult = new ShortestPathResult();

                    //để tính min graph các node tiếp theo 
                    foreach (var key in arriveList.Keys.ToList())
                    {
                        var tmp = graph.Dijkstra(startNode, key);
                        int distance = tmp.Distance;
                        // tính khoảng cách

                        if (distance < minDistance && arriveList[key] == false)
                        // nếu thỏa mãn bé hơn và không lặp node, chưa đi qua thì với vô if này
                        {
                            minDistance = distance;
                            nextNode = key;
                            pathResult = tmp;
                        }

                    }


                    totalDistance += minDistance;
                    result.AddRange(pathResult.GetPath().Where(x => x != startNode));
                    //cộng dồn distance để tính tổng của mỗi start node
                    startNode = nextNode;
                    // t gán điểm tiếp theo của start node là cái node vừa đi qua
                    arriveList[nextNode] = true;
                }

                var p = graph.Dijkstra(startNode, node);
                totalDistance += p.Distance;
                result.AddRange(p.GetPath().Where(x => x != startNode));
                result.RemoveAt(result.Count - 1);
                while (true)
                {
                    if (result[0] == 1)
                    {
                        break;
                    }
                    result.Add(result[0]);
                    result.RemoveAt(0);
                }
                result.Add(result[0]);

                if (totalDistance < shortestPath.Distance)
                {
                    shortestPath = new ShortestPathResponse()
                    {
                        StartNode = (int)node,
                        Distance = totalDistance,
                        Path = result
                    };
                }

            }
            return shortestPath;
        }

        //hàm trả về thời gian di chuyển giữa các phòng
        public async Task<TimeSpan> GetTimeToMoveFromRoom(List<int> roomId)
        {
            ShortestPathResponse shortestPath = await GetShortestPath(roomId);
            double distance = shortestPath.Distance;


            double weightConverted = 0.7;
            double velocityAVG = 5;

            
            //định nghĩa function tỉ lệ
            //double distanceToGo = (distance * weightConverted  * 100) /(velocityAVG* 60);

            double distanceToGo = distance * weightConverted * 60 / (velocityAVG * 1000);

            //TimeSpan convertToTime = TimeSpan.FromMinutes(distanceToGo);

            TimeSpan convertToTime = TimeSpan.FromMinutes(distanceToGo);

            return convertToTime;
        }
        
        //hàm trả về list position cần đi và text hướng dẫn đi
        public async Task<List<SuggestRouteResponse>> GetRouteBaseOnExhibitForUser(List<int> exhibitId)
        {
            //list trả về
            List<SuggestRouteResponse> listResponse = new List<SuggestRouteResponse>();
                        
            List<RoomResponse> listRoom = await _roomService.GetRoomFromListExhibit(exhibitId);

            List<int> listRoomId = new List<int>();
            foreach(var item in listRoom)
            {
                listRoomId.Add(item.Id);
            }

            ShortestPathResponse shortestPath = await GetShortestPath(listRoomId);

            //trả về list đường đi
            List<uint> listRoute = shortestPath.Path;
            //convert nó thành list int để add vào response trả về
            List<int> convertlistRoute = new List<int>();

            foreach (var item in listRoute)
            {
                convertlistRoute.Add(Convert.ToInt32(item));
            }

            //xong phần get list suggest route


            //get text route base on language
            string textRouteVi = "Lộ trình:\n";
            string textRouteEng = "Route:\n";
            //lấy list position dựa trên list route


                //text Viet
                foreach (var item in convertlistRoute)
                {
                    Position position =  _unitOfWork.Repository<Position>().GetById(item);
                    textRouteVi = textRouteVi + "-" + position.DescriptionVie + "\n";
                    textRouteEng = textRouteEng + "-" + position.DescriptionEng + "\n";
                }
            
            SuggestRouteResponse suggestRouteResponse = new SuggestRouteResponse(convertlistRoute, textRouteVi, textRouteEng);
            listResponse.Add(suggestRouteResponse);

            return listResponse.ToList();
        }

        public async Task<List<int>> GetShortestPathToBackToStartPoint(int roomId)
        {

            Graph<int, string> graph = CreateGraph();


            
            List<uint> arriveList = new List<uint>();

            //lấy thằng điểm bắt đầu của map đang hiện hành
            Position startPosition = await _unitOfWork.Repository<Position>().GetAll().
                                    Where(p => p.DescriptionEng == "Start point"
                                            && p.FloorId == p.Floor.Id
                                            && p.Floor.MapId == p.Floor.Map.Id
                                            && p.Floor.Map.Status == true).FirstOrDefaultAsync();

            

            if (startPosition != null)
            {
               
                arriveList.Add((uint)startPosition.Id);
            }

            
            var positionRoom = _unitOfWork.Repository<Position>().GetAll().Where(p => p.RoomId == roomId
                                                                                      && p.FloorId == p.Floor.Id
                                                                                      && p.Floor.MapId == p.Floor.Map.Id
                                                                                      && p.Floor.Map.Status == true).FirstOrDefault();
           


            
                
            var p = graph.Dijkstra((uint)positionRoom.Id, (uint)startPosition.Id);

            var rs = p.GetPath().ToList();

            List<int> convertRs = new List<int>();

            foreach (var item in rs)
            {
                convertRs.Add((int)item);
            }
                
            return convertRs;
        }

        public async Task<List<SuggestRouteResponse>> GetRouteToBackToStartPoint(int roomId)
        {
            List<SuggestRouteResponse> listResponse = new List<SuggestRouteResponse>();

            List<int> listRouteToBack = await GetShortestPathToBackToStartPoint(roomId);


            string textRouteVi = "Lộ trình:\n";
            string textRouteEng = "Route:\n";
            foreach (var item in listRouteToBack)
            {
                Position position = _unitOfWork.Repository<Position>().GetById(item);
                textRouteVi = textRouteVi + "-" + position.DescriptionVie + "\n";
                textRouteEng = textRouteEng + "-" + position.DescriptionEng + "\n";
            }

            SuggestRouteResponse suggestRouteResponse = new SuggestRouteResponse(listRouteToBack, textRouteVi, textRouteEng);
            listResponse.Add(suggestRouteResponse);

            return listResponse.ToList();            
        }
    }
}
