using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;
using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using eTourGuide.Service.Helpers;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.ImplService
{
    public class DurationService : IDurationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomService _roomService;
        private readonly IExhibitService _exhibitService;
        private readonly IShortestPathAndSuggestRouteService _shortestPathAndSuggestRouteService;
        public DurationService(IUnitOfWork unitOfWork, IRoomService roomService, IExhibitService exhibitService, IShortestPathAndSuggestRouteService shortestPathAndSuggestRouteService)
        {
            _roomService = roomService;
            _unitOfWork = unitOfWork;
            _exhibitService = exhibitService;
            _shortestPathAndSuggestRouteService = shortestPathAndSuggestRouteService;
        }


        //hàm trả về total time khi visitor dừng lại để xem các object đã chọn
        public  TimeSpan GetTotalTimeForVisitExhibitInEvent(int id, List<int> exhibitId)
        {
            List<ExhibitInEvent> listExInEvt = new List<ExhibitInEvent>();
            foreach (int exId in exhibitId)
            {
                var evtTrans =  _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(x => x.Status == true && x.EventId == id && x.Event.Status == (int)EventStatus.Status.Active && x.ExhibitId == exId).FirstOrDefault();
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

        public TimeSpan GetTotalTimeForVisitExhibitInTopic(int id, List<int> exhibitId)
        {
            List<eTourGuide.Data.Entity.ExhibitInTopic> listExInTopic = new List<eTourGuide.Data.Entity.ExhibitInTopic>();
            foreach (int exId in exhibitId)
            {
                var topicTrans = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll().Where(x => x.Status == true && x.TopicId == id && x.Topic.Status == (int)TopicStatus.Status.Active && x.ExhibitId == exId).FirstOrDefault();
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
       

      


        //tính tổng thời gian di chuyển và dừng lại xem hiện vật
        public async Task<TimeSpan> TotalTimeForVisitorInEvent(int id, List<int> exhibitId)
        {
            TimeSpan timeForVisitExhibit = GetTotalTimeForVisitExhibitInEvent(id, exhibitId);

            List<RoomResponse> listRoom = await _roomService.GetRoomFromListExhibit(exhibitId);
            List<int> roomId = new List<int>();
            foreach (var item in listRoom)
            {
                roomId.Add(item.Id);
            }

            //ShortestPathAndSuggestRouteService shortestPath = new ShortestPathAndSuggestRouteService(_unitOfWork, _roomService);

            TimeSpan timeToMove = await _shortestPathAndSuggestRouteService.GetTimeToMoveFromRoom(roomId);

            TimeSpan totalTime = timeToMove + timeForVisitExhibit;
            totalTime = TimeSpan.FromMinutes(Math.Ceiling(totalTime.TotalMinutes));
            return totalTime;                         

        }

        public async Task<TimeSpan> TotalTimeForVisitorInTopic(int id, List<int> exhibitId)
        {
            TimeSpan timeForVisitExhibit = GetTotalTimeForVisitExhibitInTopic(id, exhibitId);
            List<RoomResponse> listRoom = await _roomService.GetRoomFromListExhibit(exhibitId);

            List<int> roomId = new List<int>();
            foreach (var item in listRoom)
            {
                roomId.Add(item.Id);
            }

            //ShortestPathAndSuggestRouteService shortestPath =  new ShortestPathAndSuggestRouteService(_unitOfWork, _roomService);

            TimeSpan timeToMove = await _shortestPathAndSuggestRouteService.GetTimeToMoveFromRoom(roomId);

            TimeSpan totalTime = timeToMove + timeForVisitExhibit;
            totalTime = TimeSpan.FromMinutes(Math.Ceiling(totalTime.TotalMinutes));
            return totalTime;
        }
  

        //dựa vào thời gian user nhập vào để đưa ra list exhibit phù hợp
        public  async Task<List<ExhibitResponse>> SuggestExhibitFromDuration(TimeSpan time)
        {
            List<ExhibitResponse> listResponse = new List<ExhibitResponse>();

            TimeSpan duration = new TimeSpan(00, 00, 00);
            if (duration == time)
            {
                listResponse = _exhibitService.GetHightLightExhibit();
                return listResponse;
            }


            //tạo list chứa event và topic để sort theo rating
            List<SortEventAndTopicRespnse> listEventAndTopic = new List<SortEventAndTopicRespnse>();

            //lấy ra list topic
            var listTopic = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.Status ==(int) TopicStatus.Status.Active
                                                                                && DateTime.Now >= t.StartDate).ToList();
            if (listTopic.Count() > 0)
            {
                foreach(var item in listTopic)
                {
                    SortEventAndTopicRespnse sortEventAndTopicRespnse = new SortEventAndTopicRespnse()
                    {
                        Id = item.Id,
                        Rating = (double)item.Rating,
                        RoomId = (int)item.RoomId,
                        Type = "Topic"
                    };
                    listEventAndTopic.Add(sortEventAndTopicRespnse);
                }
            }

            //lấy ra list event
            var listEvent = _unitOfWork.Repository<Event>().GetAll().Where(e => e.Status == (int) EventStatus.Status.Active
                                                                                && DateTime.Now >= e.StartDate
                                                                                && DateTime.Now <= e.EndDate).ToList();
            if (listEvent.Count() > 0)
            {
                foreach (var item in listEvent)
                {
                    SortEventAndTopicRespnse sortEventAndTopicRespnse = new SortEventAndTopicRespnse()
                    {
                        Id = item.Id,
                        Rating = (double)item.Rating,
                        RoomId = (int)item.RoomId,
                        Type = "Event"
                    };
                    listEventAndTopic.Add(sortEventAndTopicRespnse);
                }
            }

            //sort list theo rating
            listEventAndTopic = listEventAndTopic.OrderByDescending(sort => sort.Rating).ToList();

            
            List<int> roomId = new List<int>();

            TimeSpan timeToVisitExhibit = new TimeSpan(00, 00, 00);
            TimeSpan timeToMove = new TimeSpan(00, 00, 00);
            //ShortestPathAndSuggestRouteService shortestPathAndSuggestRoute = new ShortestPathAndSuggestRouteService(_unitOfWork, _roomService);

            if (listEventAndTopic.Count() > 0)
            {
              
                    
                    foreach (var item in listEventAndTopic)
                    {

                        if (item.Type == "Topic")
                        {
                            var exhibitInTopic = _unitOfWork.Repository<ExhibitInTopic>()
                                                .GetAll().Where(e => e.TopicId == item.Id).ToList();

                            roomId.Add(item.RoomId);                          
                            if (exhibitInTopic.Count() > 0)
                            {                             
                                foreach(var item2 in exhibitInTopic)
                                {
                                    ExhibitResponse exhibitResponse = new ExhibitResponse()
                                    {
                                        Id = item2.Exhibit.Id,
                                        Name = item2.Exhibit.Name,
                                        Description = item2.Exhibit.Description,
                                        NameEng = item2.Exhibit.NameEng,
                                        DescriptionEng = item2.Exhibit.DescriptionEng,
                                        Image = item2.Exhibit.Image,
                                        Rating = (double)item2.Topic.Rating,
                                        Duration = (TimeSpan)item2.Exhibit.Duration
                                    };
                                    listResponse.Add(exhibitResponse);

                                    //xét điều kiện thời gian xem và thời gian di chuyển
                                    timeToVisitExhibit = (TimeSpan)(timeToVisitExhibit +item2.Exhibit.Duration);
                                    timeToMove = await _shortestPathAndSuggestRouteService.GetTimeToMoveFromRoom(roomId);
                                    duration = timeToMove + timeToVisitExhibit;  
                                    if (duration >= time)
                                    {
                                        return listResponse;
                                    }
                                }
                            }
                        }

                        if (item.Type == "Event")
                        {
                            var exhibitInEvent = _unitOfWork.Repository<ExhibitInEvent>()
                                                .GetAll().Where(e => e.EventId == item.Id).ToList();

                            roomId.Add(item.RoomId);
                            if (exhibitInEvent.Count() > 0)
                            {
                                foreach (var item2 in exhibitInEvent)
                                {
                                    ExhibitResponse exhibitResponse = new ExhibitResponse()
                                    {
                                        Id = item2.Exhibit.Id,
                                        Name = item2.Exhibit.Name,
                                        Description = item2.Exhibit.Description,
                                        NameEng = item2.Exhibit.NameEng,
                                        DescriptionEng = item2.Exhibit.DescriptionEng,
                                        Image = item2.Exhibit.Image,
                                        Rating = (double)item2.Event.Rating,
                                        Duration = (TimeSpan)item2.Exhibit.Duration
                                    };
                                    listResponse.Add(exhibitResponse);
                                    //xét điều kiện thời gian xem và thời gian di chuyển
                                    timeToVisitExhibit = (TimeSpan)(timeToVisitExhibit + item2.Exhibit.Duration);
                                    timeToMove = await _shortestPathAndSuggestRouteService.GetTimeToMoveFromRoom(roomId);
                                    duration =  timeToMove + timeToVisitExhibit;
                                    if (duration >= time)
                                    {
                                        return listResponse;
                                    }
                                }
                            }
                        }
                    }
                             
            }
            return listResponse;
        }


        //trả về list route và text route hướng dẫn đường đi for user
        public async Task<List<SuggestRouteResponse>> GetRouteBaseOnExhibit(List<int> exhibitId)
        {
            //ShortestPathAndSuggestRouteService positionConnectForMap = new ShortestPathAndSuggestRouteService(_unitOfWork, _roomService);

            List<SuggestRouteResponse> rs = await _shortestPathAndSuggestRouteService.GetRouteBaseOnExhibitForUser(exhibitId);

            return rs;
        }

        
    }
}
