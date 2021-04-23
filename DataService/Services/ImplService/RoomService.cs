using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using eTourGuide.Service.Exceptions;
using eTourGuide.Service.Helpers;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.ImplService
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddEventToRoom(int eventId, int roomId)
        {
            int rs = 0;
            
         

            Event evt = _unitOfWork.Repository<Event>().GetById(eventId);
            Room room = _unitOfWork.Repository<Room>().GetById(roomId);

            
            
            try
            {
                evt.RoomId = room.Id;
                _unitOfWork.Repository<Event>().Update(evt, evt.Id);

                room.Status = (int) RoomStatus.Status.Added;
                _unitOfWork.Repository<Room>().Update(room, room.Id);

              
                
                await _unitOfWork.CommitAsync();

                rs = 1;
                return rs;
            }
            catch (Exception)
            {
                throw new Exception("Add Event To Room Error!!!");
            }
        }

        public async Task<int> AddTopicToRoom(int topicId, int roomId)
        {
            int rs = 0;                      

            Topic topic = _unitOfWork.Repository<Topic>().GetById(topicId);
            Room room = _unitOfWork.Repository<Room>().GetById(roomId);         
           
            try
            {
                topic.RoomId = room.Id;
                _unitOfWork.Repository<Topic>().Update(topic, topic.Id);

                room.Status = (int)RoomStatus.Status.Added;
                _unitOfWork.Repository<Room>().Update(room, room.Id);
                
                await _unitOfWork.CommitAsync();

                rs = 1;
                return rs;
            }
            catch (Exception)
            {
                throw new Exception("Add Topic To Room Error!!!");
            }
        }

        public async Task<int> DeleteEventInRoom(int eventId)
        {
            int rs = 0;
            
            Event evt = _unitOfWork.Repository<Event>().GetById(eventId);

            if (evt.Status == (int)EventStatus.Status.Active)
            {
                throw new Exception("Can not Delete Event in This Room because Event is active now!!!");
            }
                                  
            //var exhibitInEvent = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(e => e.Status == true && e.EventId == eventId).AsQueryable();
            try
            {
                Room room = _unitOfWork.Repository<Room>().GetAll().Where(r => r.Id == evt.RoomId).FirstOrDefault();
                room.Status = (int)RoomStatus.Status.Ready;
                _unitOfWork.Repository<Room>().Update(room, room.Id);

                evt.RoomId = null;
                _unitOfWork.Repository<Event>().Update(evt, evt.Id);

                await _unitOfWork.CommitAsync();
                rs = 1;
                return rs;
            }
            catch
            {
                throw new Exception("Delete Event in Room Fail!!!");
            }        
        }

        public async Task<int> DeleteTopicInRoom(int topicId)
        {
            int rs = 0;
           
            Topic topic = _unitOfWork.Repository<Topic>().GetById(topicId);

            if (topic.Status == (int)TopicStatus.Status.Active)
            {
                throw new Exception("Can not Delete Topic in This Room because Topic is active now!!!");
            }
           
           // var exhibitInTopic = _unitOfWork.Repository<ExhibitInTopic>().GetAll().Where(e => e.Status == true && e.TopicId == topicId).AsQueryable();

            try
            {
                Room room = _unitOfWork.Repository<Room>().GetAll().Where(r => r.Id == topic.RoomId).FirstOrDefault();
                room.Status = (int)RoomStatus.Status.Ready;
                _unitOfWork.Repository<Room>().Update(room, room.Id);


                topic.RoomId = null;
                _unitOfWork.Repository<Topic>().Update(topic, topic.Id);


                await _unitOfWork.CommitAsync();
                rs = 1;
                return rs;
            }
            catch
            {
                throw new Exception("Delete Topic in Room Fail!!!");
            }

        }

        public List<RoomResponse> GetAllRoom()
        {
            var rs = _unitOfWork.Repository<Room>().GetAll().Where(r => r.Status == (int)RoomStatus.Status.Added);
            List<RoomResponse> roomResponses = new List<RoomResponse>();
            foreach (var item in rs)
            {
                if (item.Events.Where(evtInRoom =>item.Id == evtInRoom.RoomId
                                                  && evtInRoom.Status == (int)EventStatus.Status.Active
                                                  && DateTime.Now >= evtInRoom.StartDate
                                                  && DateTime.Now <= evtInRoom.EndDate).FirstOrDefault() != null

                    || item.Topics.Where(topicInRoom => item.Id == topicInRoom.RoomId
                                                        && topicInRoom.Status == (int)TopicStatus.Status.Active
                                                        && DateTime.Now >= topicInRoom.StartDate).FirstOrDefault() != null)
                {
                    RoomResponse room = new RoomResponse
                    {
                        Id = item.Id,
                        Floor = (int)item.Floor,
                        No = (int)item.No,
                        Status = (int)item.Status
                    };
                    roomResponses.Add(room);
                }       
            }
            return roomResponses.ToList();
        }

        public List<ExhibitResponse> GetExhibitFromRoom(int roomId)
        {
            //check ở nhánh Event
            var exhibitInEvent = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInEvent>()
                    .GetAll().Where(x => x.Event.RoomId == roomId 
                                         && x.Status == true 
                                         && x.Event.Status == (int)EventStatus.Status.Active
                                         && DateTime.Now >= x.Event.StartDate
                                         && DateTime.Now <= x.Event.EndDate);

            List<ExhibitResponse> listExhibitResponse = new List<ExhibitResponse>();

            if (exhibitInEvent.Count() > 0)
            {
                foreach (var item in exhibitInEvent)
                {
                    ExhibitResponse obj = new ExhibitResponse()
                    {
                        Id = item.ExhibitId,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        NameEng = item.Exhibit.NameEng,
                        DescriptionEng = item.Exhibit.DescriptionEng,
                        Image = item.Exhibit.Image,
                    };
                    listExhibitResponse.Add(obj);
                }
            }
            

            //check ở nhánh Topic
            var exhibitInTopic = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>()
                    .GetAll().Where(x => x.Topic.RoomId == roomId 
                                         && x.Status == true 
                                         && x.Topic.Status == (int)TopicStatus.Status.Active
                                         && DateTime.Now >= x.Topic.StartDate);

            if (exhibitInTopic.Count() > 0)
            {
                foreach (var item in exhibitInTopic)
                {
                    ExhibitResponse obj = new ExhibitResponse()
                    {
                        Id = item.ExhibitId,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        NameEng = item.Exhibit.NameEng,
                        DescriptionEng = item.Exhibit.DescriptionEng,
                        Image = item.Exhibit.Image,
                    };
                    listExhibitResponse.Add(obj);
                }
            }
            return listExhibitResponse;
        }
        
        public async Task<ObjectResponseInRoomForAdmin> GetTopicOrEventInRoom(int roomId)
        {

            Event evt = _unitOfWork.Repository<Event>().GetAll().Where(e => e.RoomId == roomId && e.IsDelete == false).FirstOrDefault();
            Topic topic = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.RoomId == roomId && t.IsDelete == false).FirstOrDefault();


            ObjectResponseInRoomForAdmin response = null;


            //check ở phía event in room
            if (evt != null)
            {
                DateTime createDate = (DateTime)evt.CreateDate;
                DateTime startDate = (DateTime)evt.StartDate;
                DateTime endDate = (DateTime)evt.EndDate;
                
                string statusConvert = "";

                if (evt.Status == (int) EventStatus.Status.New)
                {
                    statusConvert = "Mới";
                }
                else if (evt.Status == (int)EventStatus.Status.Waiting)
                {
                    statusConvert = "Đang chờ kích hoạt";
                }
                else if (evt.Status == (int)EventStatus.Status.Active)
                {
                    statusConvert = "Đang diễn ra";
                }
                else if (evt.Status == (int)EventStatus.Status.Disactive)
                {
                    statusConvert = "Tạm dừng";
                }
                else if (evt.Status == (int)EventStatus.Status.Closed)
                {
                    statusConvert = "Đã đóng";
                }


                response = new ObjectResponseInRoomForAdmin
                {
                    RoomId = (int)evt.RoomId,
                    EventOrTopicId = evt.Id,
                    Name = evt.Name,
                    Description = evt.Description,
                    NameEng = evt.NameEng,
                    DescriptionEng = evt.DescriptionEng,
                    Image = evt.Image,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                    Rating = (double)evt.Rating,
                    Status = statusConvert,
                    StartDate = startDate.Date.ToString("yyyy-MM-dd"),
                    EndDate = endDate.Date.ToString("yyyy-MM-dd"),
                    Type = "Event"
                };

          
            }

            //check ở phía topic in room
            if (topic != null)
            {
                DateTime createDate = (DateTime)topic.CreateDate;
                DateTime startDate = (DateTime)topic.StartDate;
                

                string statusConvert = "";

                if (topic.Status == (int) TopicStatus.Status.New)
                {
                    statusConvert = "Mới";
                }
                else if (topic.Status == (int)TopicStatus.Status.Waiting)
                {
                    statusConvert = "Đang chờ kích hoạt";
                }
                else if (topic.Status == (int)TopicStatus.Status.Active)
                {
                    statusConvert = "Đang diễn ra";
                }
                else if (topic.Status == (int)TopicStatus.Status.Disactive)
                {
                    statusConvert = "Tạm dừng";
                }
                else if (topic.Status == (int)TopicStatus.Status.Closed)
                {
                    statusConvert = "Đã đóng";
                }


                response = new ObjectResponseInRoomForAdmin
                {
                    RoomId = (int)topic.RoomId,
                    EventOrTopicId = topic.Id,
                    Name = topic.Name,
                    Description = topic.Description,
                    NameEng = topic.NameEng,
                    DescriptionEng = topic.DescriptionEng,
                    Image = topic.Image,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                    Rating = (double)topic.Rating,
                    Status = statusConvert,
                    StartDate = startDate.Date.ToString("yyyy-MM-dd"),
                    Type = "Topic"
                };
                
            }           

            if (response == null)
            {
                throw new Exception("There is no Topic or Event in this Room");
            }
            return response;

        }

        public async Task<List<RoomResponse>> GetRoomFromListExhibit(List<int> exhibitId)
        {
            //Check ở nhánh Event
            //Get List ExhibitInEvent vs ExhibitId truyền vào
            List<eTourGuide.Data.Entity.ExhibitInEvent> listExhibitInEvent = new List<eTourGuide.Data.Entity.ExhibitInEvent>();
            foreach (int item in exhibitId)
            {
                var exhibitInEvent = await _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInEvent>()
                    .GetAll().Where(x => x.ExhibitId == item && x.Status == true && x.Event.Status == (int)EventStatus.Status.Active).FirstOrDefaultAsync();
                if (exhibitInEvent != null)
                {
                    listExhibitInEvent.Add(exhibitInEvent);
                }
            }

            //Get Room 
            Dictionary<int, object> hash = new Dictionary<int, object>();
            List<RoomResponse> listRoom = new List<RoomResponse>();

            if (listExhibitInEvent != null)
            {
                foreach (var item in listExhibitInEvent)
                {
                    var room = _unitOfWork.Repository<Room>().GetAll().Where(x => x.Id == item.Event.RoomId).FirstOrDefault();
                    RoomResponse roomRs = new RoomResponse()
                    {
                        Id = room.Id,
                        Floor = (int)room.Floor,
                        No = (int)room.No,
                        Status = (int)room.Status
                    };
                    if (!hash.ContainsKey(roomRs.Id))
                    {
                        hash.Add(roomRs.Id, roomRs);
                    }
                }
            }


            //Check ở nhánh Topic
            //Get List ExhibitInTopic vs ExhibitId truyền vào
            List<eTourGuide.Data.Entity.ExhibitInTopic> listExhibitInTopic = new List<eTourGuide.Data.Entity.ExhibitInTopic>();
            foreach (int item in exhibitId)
            {
                var exhibitInTopic = await _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll().Where(x => x.ExhibitId == item && x.Status == true && x.Topic.Status == (int)TopicStatus.Status.Active).FirstOrDefaultAsync();
                if (exhibitInTopic != null)
                {
                    listExhibitInTopic.Add(exhibitInTopic);
                }
            }

            //Get Room 
            if (listExhibitInTopic != null)
            {
                foreach (var item in listExhibitInTopic)
                {
                    var room = _unitOfWork.Repository<Room>().GetAll().Where(x => x.Id == item.Topic.RoomId).FirstOrDefault();
                    RoomResponse roomRs = new RoomResponse()
                    {
                        Id = room.Id,
                        Floor = (int)room.Floor,
                        No = (int)room.No,
                        Status = (int)room.Status
                    };
                    if (!hash.ContainsKey(roomRs.Id))
                    {
                        hash.Add(roomRs.Id, roomRs);
                    }

                }
            }

            foreach (KeyValuePair<int, object> r in hash)
            {
                listRoom.Add((RoomResponse)r.Value);
            }

            return listRoom.ToList();
        }

        public List<RoomResponse> GetRoomFromFloor(int floorNo, int status)
        {
            List<RoomResponse> listRoomResponse = new List<RoomResponse>();

            //lấy những phòng chưa có event và topic
            if (status == (int) RoomStatus.Status.Ready)
            {
                var listRoomReadyFromFloor = _unitOfWork.Repository<Room>().GetAll()
                                    .Where(r => r.Status == (int)RoomStatus.Status.Ready
                                            && r.FloorNavigation.FloorNo == floorNo
                                            && r.Floor == r.FloorNavigation.Id
                                            && r.FloorNavigation.MapId == r.FloorNavigation.Map.Id
                                            && r.FloorNavigation.Map.Status == true).ToList();

                if (listRoomReadyFromFloor.Count() > 0)
                {
                    foreach (var item in listRoomReadyFromFloor)
                    {
                        RoomResponse roomResponse = new RoomResponse()
                        {
                            Id = item.Id,
                            No = (int)item.No,
                            Floor = (int)item.Floor,
                            Status = (int)item.Status
                        };
                        listRoomResponse.Add(roomResponse);
                    }
                }

            }

            //lấy những phòng đã có event hoặc topic
            if (status == (int)RoomStatus.Status.Added)
            {
                var listRoomAddedFromFloor = _unitOfWork.Repository<Room>().GetAll()
                                    .Where(r => r.Status == (int)RoomStatus.Status.Added
                                            && r.FloorNavigation.FloorNo == floorNo
                                            && r.Floor == r.FloorNavigation.Id
                                            && r.FloorNavigation.MapId == r.FloorNavigation.Map.Id
                                            && r.FloorNavigation.Map.Status == true).ToList();

                if (listRoomAddedFromFloor.Count() > 0)
                {
                    foreach (var item in listRoomAddedFromFloor)
                    {
                        RoomResponse roomResponse = new RoomResponse()
                        {
                            Id = item.Id,
                            No = (int)item.No,
                            Floor = (int)item.Floor,
                            Status = (int)item.Status
                        };
                        listRoomResponse.Add(roomResponse);
                    }
                }

            }


            //lấy tất cả
            if (status == 2)
            {
                var listRoomFromFloor = _unitOfWork.Repository<Room>().GetAll()
                                    .Where(r => r.FloorNavigation.FloorNo == floorNo
                                            && r.Floor == r.FloorNavigation.Id
                                            && r.FloorNavigation.MapId == r.FloorNavigation.Map.Id
                                            && r.FloorNavigation.Map.Status == true).ToList();

                if (listRoomFromFloor.Count() > 0)
                {
                    foreach (var item in listRoomFromFloor)
                    {
                        RoomResponse roomResponse = new RoomResponse()
                        {
                            Id = item.Id,
                            No = (int)item.No,
                            Floor = (int)item.Floor,
                            Status = (int)item.Status
                        };
                        listRoomResponse.Add(roomResponse);
                    }
                }
            }
            return listRoomResponse.ToList();
        }
    }
}
