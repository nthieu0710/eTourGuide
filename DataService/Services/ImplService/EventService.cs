using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using eTourGuide.Service.Exceptions;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.ImplService
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> AddEvent(string Name, string Description, string NameEng, string DescriptionEng, string Image, DateTime StartDate, DateTime EndDate)
        {
            int statusToDb = 0;
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);
            
            Event evt = new Event
            {
                Name = Name,
                Description = Description,
                NameEng = NameEng,
                DescriptionEng = DescriptionEng,
                Image = Image,
                CreateDate = dtnew,
                Rating = 0,
                Status = statusToDb,
                StartDate = StartDate,
                EndDate = EndDate,
                IsDelete = false,
                RoomId = null
            };
            try
            {
                await _unitOfWork.Repository<Event>().InsertAsync(evt);
                await _unitOfWork.CommitAsync();
                return evt.Id;
            }
            catch (Exception)
            {
                throw new Exception("Insert Error!!!");
            }
        }

        public async Task<int> AddExhibitToEvent(int eventId, int exhibitId)
        {
            int rs = 0;
            
            Event evt = _unitOfWork.Repository<Event>().GetById(eventId);
            
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);

            Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(exhibitId);

            ExhibitInEvent exhibitInEvent = new ExhibitInEvent
            {
                ExhibitId = exhibit.Id,
                EventId= eventId,
                RoomId = evt.RoomId,
                CreateDate = dtnew,
                Status = true
            };

            try
            {

                await _unitOfWork.Repository<ExhibitInEvent>().InsertAsync(exhibitInEvent);
                await _unitOfWork.CommitAsync();

                exhibit.Status = 1;
                _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);

                if (evt.Status == 0)
                {
                    evt.Status = 1;
                    _unitOfWork.Repository<Event>().Update(evt, evt.Id);
                }
                         
                await _unitOfWork.CommitAsync();

                rs = 1;
                return rs;
            }
            catch (Exception e)
            {

                throw new Exception("Add Exhibit To Event Error!!!");
            }
        }

        public async Task<int> DeleteEvent(int id)
        {
            Event evt = _unitOfWork.Repository<Event>().GetById(id);
            if (evt == null)
            {
                throw new Exception("Cant Not Found This Event!");
            }
            if (evt.Status == 0 || evt.Status == 1 || evt.Status == 3 || evt.Status == 4)
            {
                //xem coi có exhibit nào đang thuộc event muốn xóa hay không
                var checkExhibitInEvent = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(e => e.EventId == id).AsQueryable();
                var checkFeedbackEvent = _unitOfWork.Repository<Feedback>().GetAll().Where(f => f.EventId == id).AsQueryable();

                try
                {
                    //nếu có data trong bảng exhbit in event
                    if (checkExhibitInEvent.Count() > 0)
                    {
                        //xóa data trong bảng exhibit in event
                        _unitOfWork.Repository<ExhibitInEvent>().DeleteRange(checkExhibitInEvent);

                        foreach (var item in checkExhibitInEvent)
                        {
                            Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(item.ExhibitId);
                            //thay đổi status của exhibit thành ready
                            exhibit.Status = 0;
                            _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);
                            //await _unitOfWork.CommitAsync();
                        }

                    }

                    //nếu có data trong bảng feedback
                    if (checkFeedbackEvent.Count() > 0)
                    {
                        //xóa data của event trong bảng feedback
                        _unitOfWork.Repository<Feedback>().DeleteRange(checkFeedbackEvent);
                    }


                    int roomId = (int)evt.RoomId;
                    //nếu event đang đc chứa trong phòng thì set status lại cho phòng đó
                    if (roomId > 0)
                    {
                        Room room = _unitOfWork.Repository<Room>().GetById(roomId);
                        room.Status = 0;
                        //cập nhập room
                        _unitOfWork.Repository<Room>().Update(room, room.Id);
                    }

                    //set isDelete = true để xóa topic
                    evt.IsDelete = true;
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception e)
                {
                    
                    throw new Exception("Can not delete event!!!");
                }
            }
            else if (evt.Status == 2)
            {
                throw new Exception("Can not delete event!!!");
            }
            return evt.Id;
        }
     
        public async Task<int> UpdateEvent(int id, string Name, string Description, string NameEng, string DescriptionEng, string Image, string Status, DateTime StartDate, DateTime EndDate)
        {
            int statusToDb = 0;
            Event evt = _unitOfWork.Repository<Event>().GetById(id);
            if (evt == null)
            {
                throw new Exception("Cant Not Found This Topic!");
            }

            if (Status == "New")
            {
                statusToDb = 0;
            }
            else if (Status == "Waiting")
            {             
                statusToDb = 1;                          
            }
            else if (Status == "Active")
            {
                statusToDb = 2;
            }
            else if (Status == "Disactive")
            {
                statusToDb = 3;
            }
            else if (Status == "Closed")
            {
                statusToDb = 4;
            }
           
            try
            {
                if (statusToDb == 4)
                {
                    var checkExhibitInEvent = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(e => e.Status == true && e.EventId == id).AsQueryable();
                    if (checkExhibitInEvent.Count() > 0)
                    {
                        //_unitOfWork.Repository<ExhibitInEvent>().DeleteRange(checkExhibitInEvent);

                        foreach (var item in checkExhibitInEvent)
                        {
                            //updaet field status thành false
                            item.Status = false;


                            Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(item.ExhibitId);

                            //thay đổi status của exhibit thành ready
                            exhibit.Status = 0;
                            _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);
                            //await _unitOfWork.CommitAsync();

                        }
                        _unitOfWork.Repository<ExhibitInEvent>().UpdateRange(checkExhibitInEvent);                    
                    }
                    //check xem topic đó có đang ở room nào không để xóa ra
                    int roomNo = (int)evt.RoomId;
                    if (roomNo > 0)
                    {
                        evt.RoomId = null;

                        Room room = _unitOfWork.Repository<Room>().GetAll().Where(r => r.Id == evt.RoomId).FirstOrDefault();
                        //set status của room thành 0
                        room.Status = 0;
                        _unitOfWork.Repository<Room>().Update(room, room.Id);
                    }


                }

                evt.Name = Name;
                evt.Description = Description;
                evt.NameEng = NameEng;
                evt.DescriptionEng = DescriptionEng;
                evt.Image = Image;
                evt.StartDate = StartDate;
                evt.EndDate = EndDate;
                evt.Status = statusToDb;

                await _unitOfWork.CommitAsync();

                return evt.Id;
            }
            catch (Exception)
            {
                throw new Exception("Insert Error!!!");
            }
        }

        public async Task<int> UpdateStatusFromWatingToActive(int id)
        {
            int rs = 0;

            //tìm event by id
            Event evt = _unitOfWork.Repository<Event>().GetById(id);
            //nếu tìm k thấy thì trả lỗi msg
            if (evt == null)
            {
                throw new Exception("Cant Not Found This Event!");
            }

            //check xem nó đã đc set room chưa        
            int roomNo = (int)evt.RoomId;

            //nếu chưa có phòng thì trả msg lỗi
            if (evt.RoomId == null)
            {
                throw new Exception("You must set room for this event to active!!!");
            }


            //nếu đã có phòng
            if (roomNo > 0)
            {
                try
                {
                    evt.Status = 2;
                    await _unitOfWork.CommitAsync();

                    rs = 1;
                    return rs;
                }
                catch (Exception)
                {
                    throw new Exception("Active Error Because Some Problem From Server");
                }
            }
            else
            {
                throw new Exception("Active Error Because Some Problem From Server");
            }
        }

        public List<EventResponse> SearchEventForAdmin(string name)
        {
            List<EventResponse> listEventResponse = new List<EventResponse>();
            if (String.IsNullOrEmpty(name))
            {
                listEventResponse = GetAllEventsForAdmin();
                return listEventResponse;
            }


            string statusConvert = "";
            var evt = _unitOfWork.Repository<Event>().GetAll().Where(e => e.Name.Contains(name) && e.IsDelete == false).AsQueryable();
            
            foreach (var item in evt)
            {
                if (item.Status == 0)
                {
                    statusConvert = "New";
                }
                else if (item.Status == 1)
                {
                    statusConvert = "Waiting";
                }
                else if (item.Status == 2)
                {
                    statusConvert = "Active";
                }
                else if (item.Status == 3)
                {
                    statusConvert = "Disactive";
                }
                else if (item.Status == 4)
                {
                    statusConvert = "Closed";
                }

                string eventInRoom = "Sự kiện này chưa được thiết lập phòng";
                int roomNo = (int)item.RoomId;
                if (roomNo > 0)
                {
                    eventInRoom = "Sự kiện này đang ở phòng: " + roomNo;
                }

                DateTime createDate = (DateTime)item.CreateDate;
                DateTime startDate = (DateTime)item.StartDate;
                DateTime endDate = (DateTime)item.EndDate;

                EventResponse eventResponse = new EventResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    NameEng = item.NameEng,
                    DescriptionEng = item.DescriptionEng,
                    Image = item.Image,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                    StartDate = startDate.Date.ToString("yyyy-MM-dd"),
                    EndDate = endDate.Date.ToString("yyyy-MM-dd"),
                    Rating = Math.Round((float)item.Rating, 2),
                    Status = statusConvert,
                    isDelete = (bool)item.IsDelete,
                    RoomNo = eventInRoom
                };
                listEventResponse.Add(eventResponse);


            }
            return listEventResponse.ToList();
        }

        public List<EventResponse> GetAllEventsForAdmin()
        {
            string statusConvert = "";
            var rs = _unitOfWork.Repository<Event>().GetAll().Where(e => e.IsDelete == false).AsQueryable();
            List<EventResponse> listEventResponse = new List<EventResponse>();
            foreach (var item in rs)
            {
                if (item.Status == 0)
                {
                    statusConvert = "New";
                }
                else if (item.Status == 1)
                {
                    statusConvert = "Waiting";
                }
                else if (item.Status == 2)
                {
                    statusConvert = "Active";
                }
                else if (item.Status == 3)
                {
                    statusConvert = "Disactive";
                }
                else if (item.Status == 4)
                {
                    statusConvert = "Closed";
                }

                string eventInRoom = "Chủ đề này chưa được thiết lập phòng";
                int roomNo = (int)item.RoomId;
                if (roomNo > 0)
                {
                    eventInRoom = "Chủ đề này đang ở phòng: " + roomNo;
                }

                DateTime createDate = (DateTime)item.CreateDate;
                DateTime startDate = (DateTime)item.StartDate;
                DateTime endDate = (DateTime)item.EndDate;

                EventResponse eventResponse = new EventResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    NameEng = item.NameEng,
                    DescriptionEng = item.DescriptionEng,
                    Image = item.Image,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                    StartDate = startDate.Date.ToString("yyyy-MM-dd"),
                    EndDate = endDate.Date.ToString("yyyy-MM-dd"),
                    Rating = Math.Round((float)item.Rating, 2),
                    Status = statusConvert,
                    isDelete = (bool)item.IsDelete,
                    RoomNo = eventInRoom
                };
                listEventResponse.Add(eventResponse);


            }
            return listEventResponse.ToList();
        }

        public List<EventResponse> GetAllEventsForUser()
        {
            var rs = _unitOfWork.Repository<Event>().GetAll().Where(e => e.Status == 2 && e.IsDelete == false).AsQueryable();
            List<EventResponse> listEventResponse = new List<EventResponse>();
            foreach (var item in rs)
            {
                int count = 0;
                var evtInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.EventId == item.Id);

                if (evtInFeedback.Count() > 0)
                {
                    count = evtInFeedback.Count();


                }
                DateTime startDate = (DateTime)item.StartDate;
                DateTime endDate = (DateTime)item.EndDate;


                EventResponse eventResponse = new EventResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    NameEng = item.NameEng,
                    DescriptionEng = item.DescriptionEng,
                    Image = item.Image,
                    Rating = Math.Round((float)item.Rating, 2),
                    StartDate = startDate.Date.ToString("dd/MM/yyyy"),
                    EndDate = endDate.Date.ToString("dd/MM/yyyy"),
                    TotalFeedback = count
                };
                listEventResponse.Add(eventResponse);


            }
            return listEventResponse.ToList();
        }

        public List<EventResponse> GetCurrentEvent()
        {
            var evt = _unitOfWork.Repository<Event>().GetAll().Where(e => e.Status == 2 && e.IsDelete == false).AsQueryable();
            List<EventResponse> listRes = new List<EventResponse>();
            if (evt.Count() > 0)
            {
                foreach (var item in evt)
                {
                    int count = 0;
                    var evtInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.EventId == item.Id);

                    if (evtInFeedback.Count() > 0)
                    {
                        count = evtInFeedback.Count();
                    }



                    EventResponse res = new EventResponse();
                    if (item.StartDate <= DateTime.Now && item.EndDate >= DateTime.Now)
                    {

                        DateTime startDate = (DateTime)item.StartDate;
                        DateTime endDate = (DateTime)item.EndDate;

                        res.Id = item.Id;
                        res.Name = item.Name;
                        res.Description = item.Description;                       
                        res.NameEng = item.NameEng;
                        res.DescriptionEng = item.DescriptionEng;
                        res.Image = item.Image;
                        res.StartDate = startDate.Date.ToString("dd/MM/yyyy");
                        res.EndDate = endDate.Date.ToString("dd/MM/yyyy");
                        res.Rating = (double)item.Rating;
                        res.TotalFeedback = count;

                        listRes.Add(res);

                    }
                }
            }
            return listRes;
        }

        public List<EventResponse> GetEventHasNoRoom()
        {
            string statusConvert = "";
            var rs = _unitOfWork.Repository<Event>().GetAll().Where(e => e.IsDelete == false && e.Status != 4 && e.RoomId == null).AsQueryable();
            List<EventResponse> listEventResponse = new List<EventResponse>();
            foreach (var item in rs)
            {
                
                    if (item.Status == 0)
                    {
                        statusConvert = "New";
                    }
                    else if (item.Status == 1)
                    {
                        statusConvert = "Waiting";
                    }
                    else if (item.Status == 2)
                    {
                        statusConvert = "Active";
                    }
                    else if (item.Status == 3)
                    {
                        statusConvert = "Disactive";
                    }
                    else if (item.Status == 4)
                    {
                        statusConvert = "Closed";
                    }
                    string evntInRoom = "Sự kiện này chưa được thiết lập phòng";

                    DateTime createDate = (DateTime)item.CreateDate;
                    DateTime startDate = (DateTime)item.StartDate;
                    DateTime endDate = (DateTime)item.EndDate;
                    EventResponse eventResponse = new EventResponse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        NameEng = item.NameEng,
                        DescriptionEng = item.DescriptionEng,
                        Image = item.Image,
                        CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                        StartDate = startDate.Date.ToString("yyyy-MM-dd"),
                        EndDate = endDate.Date.ToString("yyyy-MM-dd"),
                        Rating = Math.Round((float)item.Rating, 2),
                        Status = statusConvert,
                        isDelete = (bool)item.IsDelete,
                        RoomNo = evntInRoom
                    };
                    listEventResponse.Add(eventResponse);
                
            }
            return listEventResponse.ToList();
        }

        public List<EventResponse> GetListHightLightEvent()
        {
            int highlightRate = 4;
            var evt = _unitOfWork.Repository<Event>().GetAll().Where(e => e.Status == 2 && e.IsDelete == false).AsQueryable();
            List<EventResponse> listEvent = new List<EventResponse>();
            foreach (var item in evt)
            {
                int count = 0;
                var evtInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.EventId == item.Id);

                if (evtInFeedback.Count() > 0)
                {
                    count = evtInFeedback.Count();
                }

                DateTime startDate = (DateTime)item.StartDate;
                DateTime endDate = (DateTime)item.EndDate;

                EventResponse eventObj = new EventResponse()
                {

                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    NameEng = item.NameEng,
                    DescriptionEng = item.DescriptionEng,
                    Image = item.Image,
                    Rating = Math.Round((float)item.Rating, 2),
                    StartDate = startDate.Date.ToString("dd/MM/yyyy"),
                    EndDate = endDate.Date.ToString("dd/MM/yyyy"),
                    TotalFeedback = count

                };
                if (eventObj.Rating >= highlightRate)
                {
                    listEvent.Add(eventObj);
                }
            }
            return listEvent.ToList();
        }
    }
}
