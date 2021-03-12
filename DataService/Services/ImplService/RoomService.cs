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
            TopicInRoom topicInRoom = _unitOfWork.Repository<TopicInRoom>().GetAll().Where(x => x.RoomId == roomId).FirstOrDefault();
            EventInRoom eventInRoom = _unitOfWork.Repository<EventInRoom>().GetAll().Where(x => x.RoomId == roomId).FirstOrDefault();

            if (topicInRoom != null)
            {
                throw new Exception("Room has been added Topic");
            }

            if (eventInRoom != null)
            {
                throw new Exception("Room has been added Event");
            }

            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);

            Event evt = _unitOfWork.Repository<Event>().GetById(eventId);
            Room room = _unitOfWork.Repository<Room>().GetById(roomId);

            EventInRoom newEventInRoom = new EventInRoom
            {
                EventId = evt.Id,
                RoomId = room.Id,
                CreateDate = dtnew
            };

            try
            {

                await _unitOfWork.Repository<EventInRoom>().InsertAsync(newEventInRoom);
                await _unitOfWork.CommitAsync();

                room.Status = 1;

                _unitOfWork.Repository<Room>().Update(room, room.Id);
                await _unitOfWork.CommitAsync();

                rs = 1;
                return rs;
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Add Event To Room Error!!!");
            }
        }

        public async Task<int> AddTopicToRoom(int topicId, int roomId)
        {
            int rs = 0;
            TopicInRoom topicInRoom = _unitOfWork.Repository<TopicInRoom>().GetAll().Where(x => x.RoomId == roomId).FirstOrDefault();
            EventInRoom eventInRoom = _unitOfWork.Repository<EventInRoom>().GetAll().Where(x => x.RoomId == roomId).FirstOrDefault();

            if (topicInRoom != null)
            {
                throw new Exception("Room has been added Topic");
            }

            if (eventInRoom != null)
            {
                throw new Exception("Room has been added Event");
            }

            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);

            Topic topic = _unitOfWork.Repository<Topic>().GetById(topicId);
            Room room = _unitOfWork.Repository<Room>().GetById(roomId);

            TopicInRoom newTopicInRoom = new TopicInRoom
            {
                TopicId = topic.Id,
                RoomId = room.Id,
                CreateDate = dtnew
            };

            try
            {

                await _unitOfWork.Repository<TopicInRoom>().InsertAsync(newTopicInRoom);
                await _unitOfWork.CommitAsync();

                room.Status = 1;

                _unitOfWork.Repository<Room>().Update(room, room.Id);
                await _unitOfWork.CommitAsync();

                rs = 1;
                return rs;
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Add Topic To Room Error!!!");
            }
        }

        public async Task<int> DeleteEventInRoom(int eventId)
        {
            int rs = 0;
            //xóa row EventInRoom
            EventInRoom eventInRoom = _unitOfWork.Repository<EventInRoom>().GetAll().Where(e => e.EventId == eventId).FirstOrDefault();
            if (eventInRoom != null)
            {
                try
                {
                    Room room = _unitOfWork.Repository<Room>().GetAll().Where(r => r.Id == eventInRoom.RoomId).FirstOrDefault();

                    _unitOfWork.Repository<EventInRoom>().Delete(eventInRoom);
                    room.Status = 0;
                    _unitOfWork.Repository<Room>().Update(room, room.Id);

                    await _unitOfWork.CommitAsync();
                    rs = 1;
                    return rs;
                }
                catch
                {
                    throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Delete Event in Room Fail!!!");
                }
            }
            return rs;
        }



        public async Task<int> DeleteTopicInRoom(int topicId)
        {
            int rs = 0;
            //xóa row TopicInRoom
            TopicInRoom topicInRoom = _unitOfWork.Repository<TopicInRoom>().GetAll().Where(t => t.TopicId == topicId).FirstOrDefault();
            if (topicInRoom != null)
            {
                try
                {
                    Room room = _unitOfWork.Repository<Room>().GetAll().Where(r => r.Id == topicInRoom.RoomId).FirstOrDefault();

                    _unitOfWork.Repository<TopicInRoom>().Delete(topicInRoom);
                    room.Status = 0;
                    _unitOfWork.Repository<Room>().Update(room, room.Id);

                    await _unitOfWork.CommitAsync();
                    rs = 1;
                    return rs;
                }
                catch
                {
                    throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Delete Event in Room Fail!!!");
                }
            }
            return rs;
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

        public List<ExhibitFeedbackResponse> GetExhibitFromRoom(int roomId)
        {
            //check ở nhánh Event
            var exhibitInEvent = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInEvent>()
                    .GetAll().Where(x => x.RoomId == roomId && x.Event.Status == 2);

            List<ExhibitFeedbackResponse> listExhibitResponse = new List<ExhibitFeedbackResponse>();

            foreach (var item in exhibitInEvent)
            {
                int count = 0;
                var exhibitInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.ExhibittId == item.ExhibitId);
                if (exhibitInFeedback != null)
                {
                    count = exhibitInFeedback.Count();

                }
                ExhibitFeedbackResponse obj = new ExhibitFeedbackResponse()
                {
                    Id = item.ExhibitId,
                    Name = item.Exhibit.Name,
                    Description = item.Exhibit.Description,
                    Image = item.Exhibit.Image,
                    Rating = (double)item.Exhibit.Rating,
                    TotalFeedback = count
                };
                listExhibitResponse.Add(obj);
            }

            //check ở nhánh Topic
            var exhibitInTopic = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>()
                    .GetAll().Where(x => x.RoomId == roomId && x.Topic.Status == 2);


            foreach (var item in exhibitInTopic)
            {
                int count = 0;
                var exhibitInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.ExhibittId == item.ExhibitId);
                if (exhibitInFeedback != null)
                {
                    count = exhibitInFeedback.Count();

                }
                ExhibitFeedbackResponse obj = new ExhibitFeedbackResponse()
                {
                    Id = item.ExhibitId,
                    Name = item.Exhibit.Name,
                    Description = item.Exhibit.Description,
                    Image = item.Exhibit.Image,
                    Rating = (double)item.Exhibit.Rating,
                    TotalFeedback = count
                };
                listExhibitResponse.Add(obj);
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
                    .GetAll().Where(x => x.ExhibitId == item && x.Event.Status == 2).FirstOrDefault();
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
                var exhibitInTopic = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll().Where(x => x.ExhibitId == item && x.Topic.Status == 2).FirstOrDefault();
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

                    /*if (listRoom.Contains(roomRs) == false)
                    {
                        listRoom.Add(roomRs);
                    }*/

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
            EventInRoom eventInRoom = _unitOfWork.Repository<EventInRoom>().GetAll().Where(e => e.RoomId == roomId).FirstOrDefault();

            TopicInRoom topicInRoom = _unitOfWork.Repository<TopicInRoom>().GetAll().Where(e => e.RoomId == roomId).FirstOrDefault();

            ObjectResponseInRoomForAdmin response = null;

            //check ở phía event in room
            if (eventInRoom != null)
            {
                DateTime createDate = (DateTime)eventInRoom.Event.CreateDate;
                DateTime startDate = (DateTime)eventInRoom.Event.StartDate;
                DateTime endDate = (DateTime)eventInRoom.Event.EndDate;
                
                string statusConvert = "";

                if (eventInRoom.Event.Status == 0)
                {
                    statusConvert = "New";
                }
                else if (eventInRoom.Event.Status == 1)
                {
                    statusConvert = "Waiting";
                }
                else if (eventInRoom.Event.Status == 2)
                {
                    statusConvert = "Active";
                }
                else if (eventInRoom.Event.Status == 3)
                {
                    statusConvert = "Disactive";
                }
                else if (eventInRoom.Event.Status == 4)
                {
                    statusConvert = "Closed";
                }


                response = new ObjectResponseInRoomForAdmin
                {
                    RoomId = eventInRoom.RoomId,
                    EventOrTopicId = eventInRoom.Event.Id,
                    Name = eventInRoom.Event.Name,
                    Description = eventInRoom.Event.Description,
                    Image = eventInRoom.Event.Image,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                    Rating = (double)eventInRoom.Event.Rating,
                    Status = statusConvert,
                    StartDate = startDate.Date.ToString("yyyy-MM-dd"),
                    EndDate = endDate.Date.ToString("yyyy-MM-dd"),
                    Type = "Event"
                };
                
            }

            //check ở phía topic in room
            if (topicInRoom != null)
            {
                DateTime createDate = (DateTime)topicInRoom.Topic.CreateDate;
                DateTime startDate = (DateTime)topicInRoom.Topic.StartDate;
                

                string statusConvert = "";

                if (topicInRoom.Topic.Status == 0)
                {
                    statusConvert = "New";
                }
                else if (topicInRoom.Topic.Status == 1)
                {
                    statusConvert = "Waiting";
                }
                else if (topicInRoom.Topic.Status == 2)
                {
                    statusConvert = "Active";
                }
                else if (topicInRoom.Topic.Status == 3)
                {
                    statusConvert = "Disactive";
                }
                else if (topicInRoom.Topic.Status == 4)
                {
                    statusConvert = "Closed";
                }


                response = new ObjectResponseInRoomForAdmin
                {
                    RoomId = topicInRoom.RoomId,
                    EventOrTopicId = topicInRoom.Topic.Id,
                    Name = topicInRoom.Topic.Name,
                    Description = topicInRoom.Topic.Description,
                    Image = topicInRoom.Topic.Image,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                    Rating = (double)topicInRoom.Topic.Rating,
                    Status = statusConvert,
                    StartDate = startDate.Date.ToString("yyyy-MM-dd"),
                    Type = "Topic"
                };

            }

            /*if (response == null)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "There's no Topic or Event in this Room!!!");
            }*/
            return response;    
        }

    }
}
