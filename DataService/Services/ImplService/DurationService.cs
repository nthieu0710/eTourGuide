using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;
using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace eTourGuide.Service.Services.ImplService
{
    public class DurationService : IDurationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomService _roomService;
        public DurationService(IUnitOfWork unitOfWork, IRoomService roomService)
        {
            _roomService = roomService;
            _unitOfWork = unitOfWork;
        }


        //hàm trả về total time khi visitor dừng lại để xem các object đã chọn
        public TimeSpan GetTotalTimeForVisitExhibitInEvent(int id, int[] exhibitId)
        {
            List<ExhibitInEvent> listExInEvt = new List<ExhibitInEvent>();
            foreach (int exId in exhibitId)
            {
                var evtTrans = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(x => x.EventId == id && x.Event.Status == 2 && x.ExhibitId == exId).FirstOrDefault();
                if (evtTrans != null)
                {
                    listExInEvt.Add(evtTrans);
                }

            }
            TimeSpan duration = new TimeSpan(00, 00, 00);
            if (listExInEvt != null)
            {
                foreach (var item in listExInEvt)
                {
                    if (item.Exhibit.Duration != null)
                    {
                        duration = (TimeSpan)(duration + item.Exhibit.Duration);
                    }
                }
            }
            return duration;
        }

        public TimeSpan GetTotalTimeForVisitExhibitInTopic(int id, int[] exhibitId)
        {
            List<eTourGuide.Data.Entity.ExhibitInTopic> listExInTopic = new List<eTourGuide.Data.Entity.ExhibitInTopic>();
            foreach (int exId in exhibitId)
            {
                var topicTrans = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll().Where(x => x.TopicId == id && x.Topic.Status == 2 && x.ExhibitId == exId).FirstOrDefault();
                if (topicTrans != null)
                {
                    listExInTopic.Add(topicTrans);
                }

            }
            TimeSpan duration = new TimeSpan(00, 00, 00);
            if (listExInTopic != null)
            {
                foreach (var item in listExInTopic)
                {
                    if (item.Exhibit.Duration != null)
                    {
                        duration = (TimeSpan)(duration + item.Exhibit.Duration);
                    }
                }
            }
            return duration;
        }


        //Hàm trả về list Exhibit khi user chọn thời gian xem
        public List<ExhibitResponse> SuggestExhibitFromDuration(TimeSpan time)
        {
            var rs = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Status == 1).AsQueryable();
            rs = rs.OrderByDescending(exhibit => exhibit.Rating);
            TimeSpan duration = new TimeSpan(00, 00, 00);
            List<ExhibitResponse> listExhibitForDuration = new List<ExhibitResponse>();
            foreach (var item in rs)
            {
                int count = 0;
                var exhibitInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.ExhibittId == item.Id);
                if (exhibitInFeedback != null)
                {
                    count = exhibitInFeedback.Count();
                }
                ExhibitResponse exhibitRes = new ExhibitResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Image = item.Image,
                    Rating = (double)item.Rating,
                    TotalFeedback = count
                };
                listExhibitForDuration.Add(exhibitRes);
            }
            //Trường hợp User nhập vào time == 0
            if (duration == time)
            {
                return listExhibitForDuration.ToList();
            }

            //Trường hợp User nhập vào time != 0

            while (duration <= time)
            {
                listExhibitForDuration = new List<ExhibitResponse>();
                foreach (var item in rs)
                {
                    duration = (TimeSpan)(duration + item.Duration);
                    if (duration > time)
                    {
                        break;
                    }
                    int count = 0;
                    var exhibitInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.ExhibittId == item.Id);
                    if (exhibitInFeedback != null)
                    {
                        count = exhibitInFeedback.Count();
                    }
                    ExhibitResponse exhibitRes = new ExhibitResponse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Image = item.Image,
                        Rating = (double)item.Rating,
                        TotalFeedback = count
                    };
                    listExhibitForDuration.Add(exhibitRes);
                }
            }

            return listExhibitForDuration.ToList();
        }

        public List<int> SuggestRouteBaseOnExhibit(int[] exhibitId)
        {
            List<RoomResponse> listRoom = _roomService.GetRoomForExhibit(exhibitId);
            int[] room = new int[listRoom.Count()];
            for (int i = 0; i < listRoom.Count; i++)
            {
                room[i] = listRoom.ElementAt(i).Id;
            }
            List<int> rs = GetListUint(room);
            return rs;
        }

        //tính quãng đường di chuyển giữa các phòng
        public double TotalDistance(int[] room)
        {
            var graph = new Graph<int, string>();
            int totalNode = 82;
            for (int i = 0; i <= totalNode; i++)
            {
                graph.AddNode(i);
            }

            graph.Connect(1, 2, 7, "Point 1 to Point 2");
            graph.Connect(2, 1, 7, "Point 2 to Point 1");

            graph.Connect(2, 3, 8, "Point 2 to Point 3");
            graph.Connect(3, 2, 8, "Point 3 to Point 2");

            graph.Connect(3, 4, 2, "Point 3 to Room 2");
            graph.Connect(4, 3, 2, "Room 2 to Point 3");

            graph.Connect(4, 5, 6, "Room 2 to Room 4");
            graph.Connect(5, 4, 6, "Room 4 to Room 2");

            graph.Connect(5, 6, 8, "Room 4 to Room 6");
            graph.Connect(6, 5, 8, "Room 6 to Room 4");

            graph.Connect(1, 7, 23, "Point 1 to Point 7");
            graph.Connect(7, 1, 23, "Point 7 to Point 1");

            graph.Connect(7, 6, 4, "Point 7 to Room 6");
            graph.Connect(6, 7, 4, "Room 6 to Point 7");

            graph.Connect(7, 8, 6, "Point 7 to Room 7");
            graph.Connect(8, 7, 6, "Room 7 to Point 7");

            graph.Connect(8, 9, 4, "Room 7 to Room 8");
            graph.Connect(9, 8, 4, "Room 8 to Room 7");

            graph.Connect(9, 10, 3, "Room 8 to Room 9");
            graph.Connect(10, 9, 3, "Room 9 to Room 8");

            graph.Connect(10, 15, 6, "Room 9 to Point 5");
            graph.Connect(15, 10, 6, "Point 5 to Room 9");

            graph.Connect(3, 11, 2, "Point 3 to Room 1");
            graph.Connect(11, 3, 2, "Room 1 to Point 3");

            graph.Connect(11, 12, 6, "Room 1 to Room 3");
            graph.Connect(12, 11, 6, "Room 3 to Room 1");

            graph.Connect(12, 13, 8, "Room 3 to Room 5");
            graph.Connect(13, 12, 8, "Room 5 to Room 3");

            graph.Connect(13, 14, 4, "Room 5 to Point 4");
            graph.Connect(14, 13, 4, "Point 4 to Room 5");

            graph.Connect(14, 15, 4, "Point 4 to Point 5");
            graph.Connect(15, 14, 4, "Point 5 to Point 4");

            graph.Connect(15, 16, 5, "Point 5 to Room 10");
            graph.Connect(16, 15, 5, "Room 10 to Point 5");

            graph.Connect(16, 17, 6, "Room 10 to Room 11");
            graph.Connect(17, 16, 6, "Room 11 to Room 10");

            graph.Connect(16, 18, 6, "Room 10 to Room 12");
            graph.Connect(18, 16, 6, "Room 12 to Room 10");

            graph.Connect(17, 19, 6, "Room 11 to Room 13");
            graph.Connect(19, 17, 6, "Room 13 to Room 11");

            graph.Connect(18, 19, 6, "Room 12 to Room 13");
            graph.Connect(19, 18, 6, "Room 13 to Room 12");

            graph.Connect(2, 20, 7, "Point 2 to Room 14");
            graph.Connect(20, 2, 7, "Room 14 to Point 2");

            graph.Connect(20, 21, 4, "Room 14 to Room 15");
            graph.Connect(21, 20, 4, "Room 15 to Room 14");

            graph.Connect(21, 22, 4, "Room 15 to Room 16");
            graph.Connect(22, 21, 4, "Room 16 to Room 15");

            graph.Connect(22, 23, 4, "Room 16 to Room 17");
            graph.Connect(23, 22, 4, "Room 17 to Room 16");

            graph.Connect(23, 24, 4, "Room 17 to Room 18");
            graph.Connect(24, 23, 4, "Room 18 to Room 17");

            graph.Connect(24, 25, 6, "Room 18 to Point 9");
            graph.Connect(25, 24, 6, "Point 9 to Room 18");

            graph.Connect(25, 26, 2, "Point 9 to Room 20");
            graph.Connect(26, 25, 2, "Room 20 to Point 9");

            graph.Connect(26, 27, 4, "Room 20 to Room 21");
            graph.Connect(27, 26, 4, "Room 21 to Room 20");

            graph.Connect(25, 28, 2, "Point 9 to Room 19");
            graph.Connect(28, 25, 2, "Room 19 to Point 9");

            graph.Connect(30, 28, 3, "Point 8 to Room 19");
            graph.Connect(28, 30, 3, "Room 19 to Point 8");

            graph.Connect(29, 14, 23, "Point 6 to Point 4");
            graph.Connect(14, 29, 23, "Point 4 to Point 6");

            graph.Connect(2, 29, 6, "Point 2 to Point 6");
            graph.Connect(29, 2, 6, "Point 6 to Point 2");

            graph.Connect(29, 30, 28, "Point 6 to Point 8");
            graph.Connect(30, 29, 28, "Point 8 to Point 6");

            graph.Connect(30, 31, 4, "Point 8 to Point 10");
            graph.Connect(31, 30, 4, "Point 10 to Point 8");

            graph.Connect(31, 32, 13, "Point 10 to Point 11");
            graph.Connect(32, 31, 13, "Point 11 to Point 10");

            graph.Connect(32, 33, 2, "Point 11 to Room 22");
            graph.Connect(33, 32, 2, "Room 22 to Point 11");

            graph.Connect(32, 34, 2, "Point 11 to Room 23");
            graph.Connect(34, 32, 2, "Room 23 to Point 11");

            graph.Connect(32, 35, 2, "Point 11 to Point 12");
            graph.Connect(35, 32, 2, "Point 12 to Point 11");

            graph.Connect(35, 36, 2, "Point 12 to Room 24");
            graph.Connect(36, 35, 2, "Room 24 to Point 12");

            graph.Connect(35, 37, 2, "Point 12 to Room 25");
            graph.Connect(37, 35, 2, "Room 25 to Point 12");

            graph.Connect(35, 38, 2, "Point 12 to Point 13");
            graph.Connect(38, 35, 2, "Point 13 to Point 12");

            graph.Connect(38, 39, 2, "Point 13 to Room 26");
            graph.Connect(39, 38, 2, "Room 26 to Point 13");

            graph.Connect(38, 40, 2, "Point 13 to Room 27");
            graph.Connect(40, 38, 2, "Room 27 to Point 13");

            graph.Connect(29, 41, 18, "Point 6 to Point 20");
            graph.Connect(41, 29, 18, "Point 20 to Point 6");

            graph.Connect(41, 76, 23, "Point 20 to Point 24");
            graph.Connect(76, 41, 23, "Point 24 to Point 20");





            graph.Connect(76, 77, 4, "Point 24 to Room 51");
            graph.Connect(77, 76, 4, "Room 51 to Point 24");

            graph.Connect(77, 78, 6, "Room 51 to Room 52");
            graph.Connect(78, 77, 6, "Room 52 to Room 51");



            graph.Connect(77, 79, 6, "Room 51 to Room 53");
            graph.Connect(79, 77, 6, "Room 53 to Room 51");



            graph.Connect(79, 81, 6, "Room 53 to Room 55");
            graph.Connect(81, 79, 6, "Room 55 to Room 53");



            graph.Connect(78, 80, 6, "Room 52 to Room 54");
            graph.Connect(80, 78, 6, "Room 54 to Room 52");



            graph.Connect(80, 82, 6, "Room 54 to Room 56");
            graph.Connect(82, 80, 6, "Room 56 to Room 54");

            graph.Connect(41, 42, 6, "Point 20 to Point 21");
            graph.Connect(42, 41, 6, "Point 21 to Point 20");

            graph.Connect(43, 42, 7, "Point 22 to Point 21");
            graph.Connect(42, 43, 7, "Point 21 to Point 22");


            graph.Connect(43, 44, 23, "Point 22 to Point 23");
            graph.Connect(44, 43, 23, "Point 23 to Point 22");


            graph.Connect(44, 45, 4, "Point 23 to Room 49");
            graph.Connect(45, 44, 4, "Room 49 to Point 23");


            graph.Connect(45, 46, 7, "Room 49 to Room 47");
            graph.Connect(46, 45, 7, "Room 47 to Room 49");

            graph.Connect(45, 48, 5, "Room 49 to Room 50");
            graph.Connect(48, 45, 5, "Room 50 to Room 49");


            graph.Connect(46, 47, 5, "Room 47 to Room 48");
            graph.Connect(47, 46, 5, "Room 48 to Room 47");


            graph.Connect(47, 48, 7, "Room 48 to Room 50");
            graph.Connect(48, 47, 7, "Room 50 to Room 48");


            graph.Connect(41, 49, 7, "Point 20 to Point 19");
            graph.Connect(49, 41, 7, "Point 19 to Point 20");


            graph.Connect(30, 50, 18, "Point 8 to Point 14");
            graph.Connect(50, 30, 18, "Point 14 to Point 8");

            graph.Connect(50, 51, 16, "Point 14 to Point 15");
            graph.Connect(51, 50, 16, "Point 15 to Point 14");

            graph.Connect(51, 52, 2, "Point 15 to Room 37");
            graph.Connect(52, 51, 2, "Room 37 to Point 15");

            graph.Connect(51, 53, 2, "Point 15 to Room 38");
            graph.Connect(53, 51, 2, "Room 38 to Point 15");

            graph.Connect(54, 51, 4, "Point 16 to Point 15");
            graph.Connect(51, 54, 4, "Point 15 to Point 16");

            graph.Connect(54, 55, 2, "Point 16 to Room 39");
            graph.Connect(55, 54, 2, "Room 39 to Point 16");

            graph.Connect(54, 56, 2, "Point 16 to Room 40");
            graph.Connect(56, 54, 2, "Room 40 to Point 16");

            graph.Connect(42, 57, 8, "Point 21 to Point 27");
            graph.Connect(57, 42, 8, "Point 27 to Point 21");

            graph.Connect(57, 58, 2, "Point 27 to Room 42");
            graph.Connect(58, 57, 2, "Room 42 to Point 27");

            graph.Connect(58, 59, 6, "Room 42 to Room 44");
            graph.Connect(59, 58, 6, "Room 44 to Room 42");

            graph.Connect(59, 60, 8, "Room 44 to Room 46");
            graph.Connect(60, 59, 8, "Room 46 to Room 44");

            graph.Connect(44, 60, 4, "Point 23 to Room 46");
            graph.Connect(60, 44, 4, "Room 46 to Point 23");

            graph.Connect(57, 61, 2, "Point 27 to Room 41");
            graph.Connect(61, 57, 2, "Room 41 to Point 27");

            graph.Connect(61, 62, 6, "Room 41 to Room 43");
            graph.Connect(62, 61, 6, "Room 43 to Room 41");

            graph.Connect(62, 63, 8, "Room 43 to Room 45");
            graph.Connect(63, 62, 8, "Room 45 to Room 43");

            graph.Connect(63, 76, 4, "Room 45 to Point 24");
            graph.Connect(76, 63, 4, "Point 24 to Room 45");

            graph.Connect(42, 64, 7, "Point 21 to Point 26");
            graph.Connect(64, 42, 7, "Point 26 to Point 21");

            graph.Connect(64, 65, 2, "Point 26 to Room 33");
            graph.Connect(65, 64, 2, "Room 33 to Point 26");


            graph.Connect(49, 65, 4, "Point 19 to Room 33");
            graph.Connect(65, 49, 4, "Room 33 to Point 19");


            graph.Connect(65, 66, 6, "Room 33 to Room 32");
            graph.Connect(66, 65, 6, "Room 32 to Room 33");


            graph.Connect(66, 67, 8, "Room 32 to Room 31");
            graph.Connect(67, 66, 8, "Room 31 to Room 32");

            graph.Connect(67, 68, 6, "Room 31 to Room 28");
            graph.Connect(68, 67, 6, "Room 28 to Room 31");

            graph.Connect(68, 50, 4, "Room 28 to Point 14");
            graph.Connect(50, 68, 4, "Point 14 to Room 28");

            graph.Connect(68, 69, 4, "Room 28 to Room 29");
            graph.Connect(69, 68, 4, "Room 29 to Room 28");


            graph.Connect(69, 70, 6, "Room 29 to Room 30");
            graph.Connect(70, 69, 6, "Room 30 to Room 29");


            graph.Connect(70, 71, 7, "Room 30 to Point 17");
            graph.Connect(71, 70, 7, "Point 17 to Room 30");

            graph.Connect(64, 72, 2, "Point 26 to Room 36");
            graph.Connect(72, 64, 2, "Room 36 to Point 26");

            graph.Connect(72, 73, 4, "Room 36 to Point 18");
            graph.Connect(73, 72, 4, "Point 18 to Room 36");

            graph.Connect(71, 73, 11, "Point 17 to Point 18");
            graph.Connect(73, 71, 11, "Point 18 to Point 17");

            graph.Connect(43, 73, 7, "Point 22 to Point 18");
            graph.Connect(73, 43, 7, "Point 18 to Point 22");

            graph.Connect(72, 74, 6, "Room 36 to Room 35");
            graph.Connect(74, 72, 6, "Room 35 to Room 36");

            graph.Connect(74, 75, 8, "Room 35 to Room 34");
            graph.Connect(75, 74, 8, "Room 34 to Room 35");

            Dictionary<uint, bool> arriveList = new Dictionary<uint, bool>();
            double rs = 0;
            arriveList.Add(1, false);
           

            //duyệt trong list room đc nhập vào để quét table Position trong databse lấy ra node tương ứng với room, điêu kiện roomid bằng vs id truyền vào và type phải là true (true == node)
            List<int> nodeList = new List<int>();
            if (room.Count() == 1)
            {
                var p = _unitOfWork.Repository<Position>().GetAll().Where(x => x.RoomId == room[0] && x.Type == true).FirstOrDefault();
                if (p != null)
                {
                    nodeList.Add((int)p.Node);
                }
            }else
            {
                foreach (int id in room)
                {
                    var p = _unitOfWork.Repository<Position>().GetAll().Where(x => x.RoomId == id && x.Type == true).FirstOrDefault();
                    if (p != null)
                    {
                        nodeList.Add((int)p.Node);
                    }
                }
            }
            


            foreach (int node in nodeList)
            {
                arriveList.Add((uint)node, false);
            }
           

            dynamic shortestPath = new
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
                    shortestPath = new
                    {
                        StartNode = node,
                        Distance = totalDistance,
                        Path = result
                    };
                }

            }

            rs = shortestPath.Distance;
            return rs;
        }


        //tính tổng thời gian di chuyển và dừng lại xem hiện vật
        public TimeSpan TotalTimeForVisitorInEvent(int id, int[] exhibitId)
        {
            TimeSpan timeForVisitExhibit = GetTotalTimeForVisitExhibitInEvent(id, exhibitId);
            List<RoomResponse> listRoom = _roomService.GetRoomForExhibit(exhibitId);
            int[] room = new int[listRoom.Count()];
            for (int i = 0; i < listRoom.Count; i++)
            {
                room[i] = listRoom.ElementAt(i).Id;
            }

            double weightConverted = 0.7;
            double velocityAVG = 5;


            //sửa
            //định nghĩa function tỉ lệ
            double distanceToGo = (TotalDistance(room) * weightConverted * velocityAVG * 100) / 60;
            
            TimeSpan convert = TimeSpan.FromMinutes(distanceToGo);
            TimeSpan totalTime = convert + timeForVisitExhibit;

            //làm tròn só phút đến 5p
            /*totalTime = TimeSpan.FromMinutes(5 * Math.Ceiling(totalTime.TotalMinutes / 5));*/
            totalTime = TimeSpan.FromMinutes(Math.Ceiling(totalTime.TotalMinutes));
            return totalTime;
        }

        public TimeSpan TotalTimeForVisitorInTopic(int id, int[] exhibitId)
        {
            TimeSpan timeForVisitExhibit = GetTotalTimeForVisitExhibitInTopic(id, exhibitId);
            List<RoomResponse> listRoom = _roomService.GetRoomForExhibit(exhibitId);
            int[] room = new int[listRoom.Count()];
            for (int i = 0; i < listRoom.Count; i++)
            {
                room[i] = listRoom.ElementAt(i).Id;
            }

            double weightConverted = 0.7;
            double velocityAVG = 5;
            double distanceToGo = (TotalDistance(room) * weightConverted * velocityAVG * 100) / 60;
            
            TimeSpan convert = TimeSpan.FromMinutes(distanceToGo);
            TimeSpan totalTime = convert + timeForVisitExhibit;

            //làm tròn só phút đến 5p
            //totalTime = TimeSpan.FromMinutes(5 * Math.Ceiling(totalTime.TotalMinutes / 5));
            totalTime = TimeSpan.FromMinutes(Math.Ceiling(totalTime.TotalMinutes));
            return totalTime;
        }




        public List<int> GetListUint(int[] room)
        {
            var graph = new Graph<int, string>();
            int totalNode = 82;
            for (int i = 0; i <= totalNode; i++)
            {
                graph.AddNode(i);
            }

            graph.Connect(1, 2, 7, "Point 1 to Point 2");
            graph.Connect(2, 1, 7, "Point 2 to Point 1");

            graph.Connect(2, 3, 8, "Point 2 to Point 3");
            graph.Connect(3, 2, 8, "Point 3 to Point 2");

            graph.Connect(3, 4, 2, "Point 3 to Room 2");
            graph.Connect(4, 3, 2, "Room 2 to Point 3");

            graph.Connect(4, 5, 6, "Room 2 to Room 4");
            graph.Connect(5, 4, 6, "Room 4 to Room 2");

            graph.Connect(5, 6, 8, "Room 4 to Room 6");
            graph.Connect(6, 5, 8, "Room 6 to Room 4");

            graph.Connect(1, 7, 23, "Point 1 to Point 7");
            graph.Connect(7, 1, 23, "Point 7 to Point 1");

            graph.Connect(7, 6, 4, "Point 7 to Room 6");
            graph.Connect(6, 7, 4, "Room 6 to Point 7");

            graph.Connect(7, 8, 6, "Point 7 to Room 7");
            graph.Connect(8, 7, 6, "Room 7 to Point 7");

            graph.Connect(8, 9, 4, "Room 7 to Room 8");
            graph.Connect(9, 8, 4, "Room 8 to Room 7");

            graph.Connect(9, 10, 3, "Room 8 to Room 9");
            graph.Connect(10, 9, 3, "Room 9 to Room 8");

            graph.Connect(10, 15, 6, "Room 9 to Point 5");
            graph.Connect(15, 10, 6, "Point 5 to Room 9");

            graph.Connect(3, 11, 2, "Point 3 to Room 1");
            graph.Connect(11, 3, 2, "Room 1 to Point 3");

            graph.Connect(11, 12, 6, "Room 1 to Room 3");
            graph.Connect(12, 11, 6, "Room 3 to Room 1");

            graph.Connect(12, 13, 8, "Room 3 to Room 5");
            graph.Connect(13, 12, 8, "Room 5 to Room 3");

            graph.Connect(13, 14, 4, "Room 5 to Point 4");
            graph.Connect(14, 13, 4, "Point 4 to Room 5");

            graph.Connect(14, 15, 4, "Point 4 to Point 5");
            graph.Connect(15, 14, 4, "Point 5 to Point 4");

            graph.Connect(15, 16, 5, "Point 5 to Room 10");
            graph.Connect(16, 15, 5, "Room 10 to Point 5");

            graph.Connect(16, 17, 6, "Room 10 to Room 11");
            graph.Connect(17, 16, 6, "Room 11 to Room 10");

            graph.Connect(16, 18, 6, "Room 10 to Room 12");
            graph.Connect(18, 16, 6, "Room 12 to Room 10");

            graph.Connect(17, 19, 6, "Room 11 to Room 13");
            graph.Connect(19, 17, 6, "Room 13 to Room 11");

            graph.Connect(18, 19, 6, "Room 12 to Room 13");
            graph.Connect(19, 18, 6, "Room 13 to Room 12");

            graph.Connect(2, 20, 7, "Point 2 to Room 14");
            graph.Connect(20, 2, 7, "Room 14 to Point 2");

            graph.Connect(20, 21, 4, "Room 14 to Room 15");
            graph.Connect(21, 20, 4, "Room 15 to Room 14");

            graph.Connect(21, 22, 4, "Room 15 to Room 16");
            graph.Connect(22, 21, 4, "Room 16 to Room 15");

            graph.Connect(22, 23, 4, "Room 16 to Room 17");
            graph.Connect(23, 22, 4, "Room 17 to Room 16");

            graph.Connect(23, 24, 4, "Room 17 to Room 18");
            graph.Connect(24, 23, 4, "Room 18 to Room 17");

            graph.Connect(24, 25, 6, "Room 18 to Point 9");
            graph.Connect(25, 24, 6, "Point 9 to Room 18");

            graph.Connect(25, 26, 2, "Point 9 to Room 20");
            graph.Connect(26, 25, 2, "Room 20 to Point 9");

            graph.Connect(26, 27, 4, "Room 20 to Room 21");
            graph.Connect(27, 26, 4, "Room 21 to Room 20");

            graph.Connect(25, 28, 2, "Point 9 to Room 19");
            graph.Connect(28, 25, 2, "Room 19 to Point 9");

            graph.Connect(30, 28, 3, "Point 8 to Room 19");
            graph.Connect(28, 30, 3, "Room 19 to Point 8");

            graph.Connect(29, 14, 23, "Point 6 to Point 4");
            graph.Connect(14, 29, 23, "Point 4 to Point 6");

            graph.Connect(2, 29, 6, "Point 2 to Point 6");
            graph.Connect(29, 2, 6, "Point 6 to Point 2");

            graph.Connect(29, 30, 28, "Point 6 to Point 8");
            graph.Connect(30, 29, 28, "Point 8 to Point 6");

            graph.Connect(30, 31, 4, "Point 8 to Point 10");
            graph.Connect(31, 30, 4, "Point 10 to Point 8");

            graph.Connect(31, 32, 13, "Point 10 to Point 11");
            graph.Connect(32, 31, 13, "Point 11 to Point 10");

            graph.Connect(32, 33, 2, "Point 11 to Room 22");
            graph.Connect(33, 32, 2, "Room 22 to Point 11");

            graph.Connect(32, 34, 2, "Point 11 to Room 23");
            graph.Connect(34, 32, 2, "Room 23 to Point 11");

            graph.Connect(32, 35, 2, "Point 11 to Point 12");
            graph.Connect(35, 32, 2, "Point 12 to Point 11");

            graph.Connect(35, 36, 2, "Point 12 to Room 24");
            graph.Connect(36, 35, 2, "Room 24 to Point 12");

            graph.Connect(35, 37, 2, "Point 12 to Room 25");
            graph.Connect(37, 35, 2, "Room 25 to Point 12");

            graph.Connect(35, 38, 2, "Point 12 to Point 13");
            graph.Connect(38, 35, 2, "Point 13 to Point 12");

            graph.Connect(38, 39, 2, "Point 13 to Room 26");
            graph.Connect(39, 38, 2, "Room 26 to Point 13");

            graph.Connect(38, 40, 2, "Point 13 to Room 27");
            graph.Connect(40, 38, 2, "Room 27 to Point 13");

            graph.Connect(29, 41, 18, "Point 6 to Point 20");
            graph.Connect(41, 29, 18, "Point 20 to Point 6");

            graph.Connect(41, 76, 23, "Point 20 to Point 24");
            graph.Connect(76, 41, 23, "Point 24 to Point 20");





            graph.Connect(76, 77, 4, "Point 24 to Room 51");
            graph.Connect(77, 76, 4, "Room 51 to Point 24");

            graph.Connect(77, 78, 6, "Room 51 to Room 52");
            graph.Connect(78, 77, 6, "Room 52 to Room 51");



            graph.Connect(77, 79, 6, "Room 51 to Room 53");
            graph.Connect(79, 77, 6, "Room 53 to Room 51");



            graph.Connect(79, 81, 6, "Room 53 to Room 55");
            graph.Connect(81, 79, 6, "Room 55 to Room 53");



            graph.Connect(78, 80, 6, "Room 52 to Room 54");
            graph.Connect(80, 78, 6, "Room 54 to Room 52");



            graph.Connect(80, 82, 6, "Room 54 to Room 56");
            graph.Connect(82, 80, 6, "Room 56 to Room 54");

            graph.Connect(41, 42, 6, "Point 20 to Point 21");
            graph.Connect(42, 41, 6, "Point 21 to Point 20");

            graph.Connect(43, 42, 7, "Point 22 to Point 21");
            graph.Connect(42, 43, 7, "Point 21 to Point 22");


            graph.Connect(43, 44, 23, "Point 22 to Point 23");
            graph.Connect(44, 43, 23, "Point 23 to Point 22");


            graph.Connect(44, 45, 4, "Point 23 to Room 49");
            graph.Connect(45, 44, 4, "Room 49 to Point 23");


            graph.Connect(45, 46, 7, "Room 49 to Room 47");
            graph.Connect(46, 45, 7, "Room 47 to Room 49");

            graph.Connect(45, 48, 5, "Room 49 to Room 50");
            graph.Connect(48, 45, 5, "Room 50 to Room 49");


            graph.Connect(46, 47, 5, "Room 47 to Room 48");
            graph.Connect(47, 46, 5, "Room 48 to Room 47");


            graph.Connect(47, 48, 7, "Room 48 to Room 50");
            graph.Connect(48, 47, 7, "Room 50 to Room 48");


            graph.Connect(41, 49, 7, "Point 20 to Point 19");
            graph.Connect(49, 41, 7, "Point 19 to Point 20");


            graph.Connect(30, 50, 18, "Point 8 to Point 14");
            graph.Connect(50, 30, 18, "Point 14 to Point 8");

            graph.Connect(50, 51, 16, "Point 14 to Point 15");
            graph.Connect(51, 50, 16, "Point 15 to Point 14");

            graph.Connect(51, 52, 2, "Point 15 to Room 37");
            graph.Connect(52, 51, 2, "Room 37 to Point 15");

            graph.Connect(51, 53, 2, "Point 15 to Room 38");
            graph.Connect(53, 51, 2, "Room 38 to Point 15");

            graph.Connect(54, 51, 4, "Point 16 to Point 15");
            graph.Connect(51, 54, 4, "Point 15 to Point 16");

            graph.Connect(54, 55, 2, "Point 16 to Room 39");
            graph.Connect(55, 54, 2, "Room 39 to Point 16");

            graph.Connect(54, 56, 2, "Point 16 to Room 40");
            graph.Connect(56, 54, 2, "Room 40 to Point 16");

            graph.Connect(42, 57, 8, "Point 21 to Point 27");
            graph.Connect(57, 42, 8, "Point 27 to Point 21");

            graph.Connect(57, 58, 2, "Point 27 to Room 42");
            graph.Connect(58, 57, 2, "Room 42 to Point 27");

            graph.Connect(58, 59, 6, "Room 42 to Room 44");
            graph.Connect(59, 58, 6, "Room 44 to Room 42");

            graph.Connect(59, 60, 8, "Room 44 to Room 46");
            graph.Connect(60, 59, 8, "Room 46 to Room 44");

            graph.Connect(44, 60, 4, "Point 23 to Room 46");
            graph.Connect(60, 44, 4, "Room 46 to Point 23");

            graph.Connect(57, 61, 2, "Point 27 to Room 41");
            graph.Connect(61, 57, 2, "Room 41 to Point 27");

            graph.Connect(61, 62, 6, "Room 41 to Room 43");
            graph.Connect(62, 61, 6, "Room 43 to Room 41");

            graph.Connect(62, 63, 8, "Room 43 to Room 45");
            graph.Connect(63, 62, 8, "Room 45 to Room 43");

            graph.Connect(63, 76, 4, "Room 45 to Point 24");
            graph.Connect(76, 63, 4, "Point 24 to Room 45");

            graph.Connect(42, 64, 7, "Point 21 to Point 26");
            graph.Connect(64, 42, 7, "Point 26 to Point 21");

            graph.Connect(64, 65, 2, "Point 26 to Room 33");
            graph.Connect(65, 64, 2, "Room 33 to Point 26");


            graph.Connect(49, 65, 4, "Point 19 to Room 33");
            graph.Connect(65, 49, 4, "Room 33 to Point 19");


            graph.Connect(65, 66, 6, "Room 33 to Room 32");
            graph.Connect(66, 65, 6, "Room 32 to Room 33");


            graph.Connect(66, 67, 8, "Room 32 to Room 31");
            graph.Connect(67, 66, 8, "Room 31 to Room 32");

            graph.Connect(67, 68, 6, "Room 31 to Room 28");
            graph.Connect(68, 67, 6, "Room 28 to Room 31");

            graph.Connect(68, 50, 4, "Room 28 to Point 14");
            graph.Connect(50, 68, 4, "Point 14 to Room 28");

            graph.Connect(68, 69, 4, "Room 28 to Room 29");
            graph.Connect(69, 68, 4, "Room 29 to Room 28");


            graph.Connect(69, 70, 6, "Room 29 to Room 30");
            graph.Connect(70, 69, 6, "Room 30 to Room 29");


            graph.Connect(70, 71, 7, "Room 30 to Point 17");
            graph.Connect(71, 70, 7, "Point 17 to Room 30");

            graph.Connect(64, 72, 2, "Point 26 to Room 36");
            graph.Connect(72, 64, 2, "Room 36 to Point 26");

            graph.Connect(72, 73, 4, "Room 36 to Point 18");
            graph.Connect(73, 72, 4, "Point 18 to Room 36");

            graph.Connect(71, 73, 11, "Point 17 to Point 18");
            graph.Connect(73, 71, 11, "Point 18 to Point 17");

            graph.Connect(43, 73, 7, "Point 22 to Point 18");
            graph.Connect(73, 43, 7, "Point 18 to Point 22");

            graph.Connect(72, 74, 6, "Room 36 to Room 35");
            graph.Connect(74, 72, 6, "Room 35 to Room 36");

            graph.Connect(74, 75, 8, "Room 35 to Room 34");
            graph.Connect(75, 74, 8, "Room 34 to Room 35");

            Dictionary<uint, bool> arriveList = new Dictionary<uint, bool>();
            
            arriveList.Add(1, false);


            //duyệt trong list room đc nhập vào để quét table Position trong databse lấy ra node tương ứng với room, điêu kiện roomid bằng vs id truyền vào và type phải là true (true == node)
            List<int> nodeList = new List<int>();
            if (room.Count() == 1)
            {
                var p = _unitOfWork.Repository<Position>().GetAll().Where(x => x.RoomId == room[0] && x.Type == true).FirstOrDefault();
                if (p != null)
                {
                    nodeList.Add((int)p.Node);
                }
            }
            else
            {
                foreach (int id in room)
                {
                    var p = _unitOfWork.Repository<Position>().GetAll().Where(x => x.RoomId == id && x.Type == true).FirstOrDefault();
                    if (p != null)
                    {
                        nodeList.Add((int)p.Node);
                    }
                }
            }



            foreach (int node in nodeList)
            {
                arriveList.Add((uint)node, false);
            }


            dynamic shortestPath = new
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
                    shortestPath = new
                    {
                        StartNode = node,
                        Distance = totalDistance,
                        Path = result
                    };
                }

            }

            List<uint> rs = shortestPath.Path;

            List<int> convert = new List<int>();
            foreach (var item in rs)
            {
                convert.Add(Convert.ToInt32(item));
            }
            return convert;
        }

    }
}
