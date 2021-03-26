using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using eTourGuide.Service.Exceptions;
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

            
            var exhibitInEvent = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(e => e.Status == true && e.EventId == eventId).AsQueryable();
            try
            {
                evt.RoomId = room.Id;
                _unitOfWork.Repository<Event>().Update(evt, evt.Id);

                room.Status = 1;
                _unitOfWork.Repository<Room>().Update(room, room.Id);

                if (exhibitInEvent.Count() != 0)
                {
                    foreach (var item in exhibitInEvent)
                    {
                        item.RoomId = room.Id;
                    }
                    _unitOfWork.Repository<ExhibitInEvent>().UpdateRange(exhibitInEvent);
                }
                
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

            var exhibitInTopic = _unitOfWork.Repository<ExhibitInTopic>().GetAll().Where(e => e.Status == true && e.TopicId == topicId).AsQueryable();

            try
            {
                topic.RoomId = room.Id;
                _unitOfWork.Repository<Topic>().Update(topic, topic.Id);

                room.Status = 1;
                _unitOfWork.Repository<Room>().Update(room, room.Id);


                if (exhibitInTopic.Count() != 0)
                {
                    foreach (var item in exhibitInTopic)
                    {
                        item.RoomId = topic.RoomId;
                    }
                     _unitOfWork.Repository<ExhibitInTopic>().UpdateRange(exhibitInTopic);
                }
                
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

            if (evt.Status == 2)
            {
                throw new Exception("Can not Delete Event in This Room because Event is active now!!!");
            }
                                  
            var exhibitInEvent = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(e => e.Status == true && e.EventId == eventId).AsQueryable();
            try
            {
                evt.RoomId = null;
                _unitOfWork.Repository<Event>().Update(evt, evt.Id);

                if (exhibitInEvent.Count() > 0)
                {
                    foreach (var item in exhibitInEvent)
                    {
                        item.RoomId = null;
                    }
                        _unitOfWork.Repository<ExhibitInEvent>().UpdateRange(exhibitInEvent);
                }

                Room room = _unitOfWork.Repository<Room>().GetAll().Where(r => r.Id == evt.RoomId).FirstOrDefault();                   
                room.Status = 0;
                _unitOfWork.Repository<Room>().Update(room, room.Id);

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

            if (topic.Status == 2)
            {
                throw new Exception("Can not Delete Topic in This Room because Topic is active now!!!");
            }
           
            var exhibitInTopic = _unitOfWork.Repository<ExhibitInTopic>().GetAll().Where(e => e.Status == true && e.TopicId == topicId).AsQueryable();

            try
            {
                topic.RoomId = null;
                _unitOfWork.Repository<Topic>().Update(topic, topic.Id);

                if (exhibitInTopic.Count() > 0)
                {
                    foreach (var item in exhibitInTopic)
                    {
                        item.RoomId = null;
                    }
                    _unitOfWork.Repository<ExhibitInTopic>().UpdateRange(exhibitInTopic);
                }


                Room room = _unitOfWork.Repository<Room>().GetAll().Where(r => r.Id == topic.RoomId).FirstOrDefault();                   
                room.Status = 0;
                _unitOfWork.Repository<Room>().Update(room, room.Id);

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
            var rs = _unitOfWork.Repository<Room>().GetAll().Where(r => r.Status == 1).AsQueryable();
            List<RoomResponse> roomResponses = new List<RoomResponse>();
            foreach (var item in rs)
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
            return roomResponses.ToList();
        }

        public List<ExhibitResponse> GetExhibitFromRoom(int roomId)
        {
            //check ở nhánh Event
            var exhibitInEvent = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInEvent>()
                    .GetAll().Where(x => x.RoomId == roomId && x.Status == true && x.Event.Status == 2);

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
                    .GetAll().Where(x => x.RoomId == roomId && x.Status == true && x.Topic.Status == 2);

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

        public List<RoomResponse> GetRoomForExhibit(int[] exhibitId)
        {
            //Check ở nhánh Event
            //Get List ExhibitInEvent vs ExhibitId truyền vào
            List<eTourGuide.Data.Entity.ExhibitInEvent> listExhibitInEvent = new List<eTourGuide.Data.Entity.ExhibitInEvent>();
            foreach (int item in exhibitId)
            {
                var exhibitInEvent = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInEvent>()
                    .GetAll().Where(x => x.ExhibitId == item && x.Status == true && x.Event.Status == 2).FirstOrDefault();
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
                    var room = _unitOfWork.Repository<Room>().GetAll().Where(x => x.Id == item.RoomId).FirstOrDefault();
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
                var exhibitInTopic = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll().Where(x => x.ExhibitId == item && x.Status == true && x.Topic.Status == 2).FirstOrDefault();
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
                    var room = _unitOfWork.Repository<Room>().GetAll().Where(x => x.Id == item.RoomId).FirstOrDefault();
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

        public async Task<ObjectResponseInRoomForAdmin> GetTopicOrEventInRoom(int roomId)
        {

            Event evt = _unitOfWork.Repository<Event>().GetAll().Where(e => e.RoomId == roomId).FirstOrDefault();
            Topic topic = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.RoomId == roomId).FirstOrDefault();


            ObjectResponseInRoomForAdmin response = null;


            //check ở phía event in room
            if (evt != null)
            {
                DateTime createDate = (DateTime)evt.CreateDate;
                DateTime startDate = (DateTime)evt.StartDate;
                DateTime endDate = (DateTime)evt.EndDate;
                
                string statusConvert = "";

                if (evt.Status == 0)
                {
                    statusConvert = "New";
                }
                else if (evt.Status == 1)
                {
                    statusConvert = "Waiting";
                }
                else if (evt.Status == 2)
                {
                    statusConvert = "Active";
                }
                else if (evt.Status == 3)
                {
                    statusConvert = "Disactive";
                }
                else if (evt.Status == 4)
                {
                    statusConvert = "Closed";
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

                if (topic.Status == 0)
                {
                    statusConvert = "New";
                }
                else if (topic.Status == 1)
                {
                    statusConvert = "Waiting";
                }
                else if (topic.Status == 2)
                {
                    statusConvert = "Active";
                }
                else if (topic.Status == 3)
                {
                    statusConvert = "Disactive";
                }
                else if (topic.Status == 4)
                {
                    statusConvert = "Closed";
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
            return response;    
        }









        public List<RoomResponse> GetRoomFromListExhibit(List<int> exhibitId)
        {
            //Check ở nhánh Event
            //Get List ExhibitInEvent vs ExhibitId truyền vào
            List<eTourGuide.Data.Entity.ExhibitInEvent> listExhibitInEvent = new List<eTourGuide.Data.Entity.ExhibitInEvent>();
            foreach (int item in exhibitId)
            {
                var exhibitInEvent = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInEvent>()
                    .GetAll().Where(x => x.ExhibitId == item && x.Status == true && x.Event.Status == 2).FirstOrDefault();
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
                    var room = _unitOfWork.Repository<Room>().GetAll().Where(x => x.Id == item.RoomId).FirstOrDefault();
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
                var exhibitInTopic = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll().Where(x => x.ExhibitId == item && x.Status == true && x.Topic.Status == 2).FirstOrDefault();
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
                    var room = _unitOfWork.Repository<Room>().GetAll().Where(x => x.Id == item.RoomId).FirstOrDefault();
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

    }
}
