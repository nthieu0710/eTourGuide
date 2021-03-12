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
        public async Task<int> AddEvent(string Name, string Description, string Image, DateTime StartDate, DateTime EndDate)
        {
            int statusToDb = 0;
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);
            
            Event evt = new Event
            {
                Name = Name,
                Description = Description,
                Image = Image,
                CreateDate = dtnew,
                Rating = 0,
                Status = statusToDb,
                StartDate = StartDate,
                EndDate = EndDate,
            };
            try
            {
                await _unitOfWork.Repository<Event>().InsertAsync(evt);
                await _unitOfWork.CommitAsync();
                return evt.Id;
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Insert Error!!!");
            }
        }

        public async Task<int> AddExhibitToEvent(int eventId, int exhibitId)
        {
            int rs = 0;
            EventInRoom room = _unitOfWork.Repository<EventInRoom>().GetAll().Where(r => r.EventId == eventId).FirstOrDefault();
            Event evt = _unitOfWork.Repository<Event>().GetById(eventId);
            int roomId = 0;
            if (room != null)
            {
                roomId = room.RoomId;
            }
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);

            Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(exhibitId);
            rs = 0;

            ExhibitInEvent exhibitInEvent = new ExhibitInEvent
            {
                ExhibitId = exhibit.Id,
                EventId= eventId,
                RoomId = roomId,
                CreateDate = dtnew
            };

            try
            {

                await _unitOfWork.Repository<ExhibitInEvent>().InsertAsync(exhibitInEvent);
                await _unitOfWork.CommitAsync();

                exhibit.Status = 1;
                if (evt.Status == 0)
                {
                    evt.Status = 1;
                }


                _unitOfWork.Repository<Event>().Update(evt, evt.Id);
                _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);

                await _unitOfWork.CommitAsync();

                rs = 1;
                return rs;
            }
            catch (Exception e)
            {

                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Add Object To Event Error!!!");
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
                //xem coi có object nào đang thuộc event muốn xóa hay không
                var checkExhibitInEvent = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(e => e.EventId == id).AsQueryable();
                
                try
                {
                    if (checkExhibitInEvent.Count() != 0)
                    {
                        _unitOfWork.Repository<ExhibitInEvent>().DeleteRange(checkExhibitInEvent);

                        foreach (var item in checkExhibitInEvent)
                        {
                            Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(item.ExhibitId);
                           


                            //thay đổi status của exhibit thành ready
                            exhibit.Status = 0;
                            _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);
                            //await _unitOfWork.CommitAsync();

                        }

                        EventInRoom eventInRoom = _unitOfWork.Repository<EventInRoom>().GetAll().Where(e => e.EventId == id).FirstOrDefault();
                        
                        Room room = _unitOfWork.Repository<Room>().GetAll().Where(r => r.Id == eventInRoom.RoomId).FirstOrDefault();

                        //xóa row của event trong EventInRoom
                        _unitOfWork.Repository<EventInRoom>().Delete(eventInRoom);
                       

                        //set status của room thành 0
                        room.Status = 0;
                        _unitOfWork.Repository<Room>().Update(room, room.Id);


                        //Set event isDelete == True để xóa
                        evt.IsDelete = true;
                        await _unitOfWork.CommitAsync();

                    }

                    
                }
                catch (Exception e)
                {
                    
                    throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not delete event!!!");
                }
            }
            else if (evt.Status == 2)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not delete event!!!");
            }
            return evt.Id;
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
                
                    DateTime createDate = (DateTime)item.CreateDate;
                    DateTime startDate = (DateTime)item.StartDate;
                    DateTime endDate = (DateTime)item.EndDate;
                    EventResponse eventResponse = new EventResponse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Image = item.Image,
                        CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                        StartDate = startDate.Date.ToString("yyyy-MM-dd"),
                        EndDate = endDate.Date.ToString("yyyy-MM-dd"),
                        Rating = Math.Round((float)item.Rating, 2),
                        Status = statusConvert,
                        //isDelete = (bool)item.IsDelete
                    };
                    listEventResponse.Add(eventResponse);
                

            }
            return listEventResponse.ToList();
        }

        public List<EventResponseForUser> GetAllEventsForUser()
        {
            var rs = _unitOfWork.Repository<Event>().GetAll().Where(e => e.Status == 2 && e.IsDelete == false).AsQueryable();
            List<EventResponseForUser> listEventResponse = new List<EventResponseForUser>();
            foreach (var item in rs)
            {
               
                    EventResponseForUser eventResponse = new EventResponseForUser()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Image = item.Image,
                        Rating = Math.Round((float)item.Rating,2),
                        StartDate = (DateTime)item.StartDate,
                        EndDate = (DateTime)item.EndDate
                    };
                    listEventResponse.Add(eventResponse);
                

            }
            return listEventResponse.ToList();
        }

        public List<EventFeedbackResponse> GetCurrentEvent()
        {
            var evt = _unitOfWork.Repository<Event>().GetAll().Where(e => e.Status == 2 && e.IsDelete == false).AsQueryable();
            List<EventFeedbackResponse> listRes = new List<EventFeedbackResponse>();
            if (evt != null)
            {
                foreach (var item in evt)
                {
                    int count = 0;
                    var evtInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.EventId == item.Id);
                    double ratingAVG = 0;
                    double sumRating = 0;
                    if (evtInFeedback != null)
                    {
                        count = evtInFeedback.Count();
                        foreach (var item2 in evtInFeedback)
                        {
                            sumRating = (double)(sumRating + item2.Rating);
                        }
                        if (count != 0)
                        {
                            ratingAVG = sumRating / count;
                        }

                    }
                    
                    
                    
                    EventFeedbackResponse res = new EventFeedbackResponse();
                    if (item.StartDate <= DateTime.Now && item.EndDate >= DateTime.Now)
                    {
                       
                            DateTime startDate = (DateTime)item.StartDate;
                            DateTime endDate = (DateTime)item.EndDate;
                            res.Id = item.Id;
                            res.Name = item.Name;
                            res.Description = item.Description;
                            res.Image = item.Image;
                            res.StartDate = startDate.Date.ToString("dd/MM/yyyy");
                            res.EndDate = endDate.Date.ToString("dd/MM/yyyy");
                            res.Rating = ratingAVG;
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
            var rs = _unitOfWork.Repository<Event>().GetAll().Where(e => e.IsDelete == false).AsQueryable();
            List<EventResponse> listEventResponse = new List<EventResponse>();
            foreach (var item in rs)
            {
                EventInRoom eventInRoom = _unitOfWork.Repository<EventInRoom>().GetAll().Where(e => e.EventId == item.Id).FirstOrDefault();
                if (eventInRoom == null)
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

                    DateTime createDate = (DateTime)item.CreateDate;
                    DateTime startDate = (DateTime)item.StartDate;
                    DateTime endDate = (DateTime)item.EndDate;
                    EventResponse eventResponse = new EventResponse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Image = item.Image,
                        CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                        StartDate = startDate.Date.ToString("yyyy-MM-dd"),
                        EndDate = endDate.Date.ToString("yyyy-MM-dd"),
                        Rating = Math.Round((float)item.Rating, 2),
                        Status = statusConvert,
                        //isDelete = (bool)item.IsDelete
                    };
                    listEventResponse.Add(eventResponse);
                }
            }
            return listEventResponse.ToList();
        }

        public List<EventFeedbackResponse> GetListHightLightEvent()
        {
            int highlightRate = 4;
            var evt = _unitOfWork.Repository<Event>().GetAll().Where(e => e.Status == 2 && e.IsDelete == false).AsQueryable();
            List<EventFeedbackResponse> listEvent = new List<EventFeedbackResponse>();
            foreach (var item in evt)
            {
                int count = 0;
                var evtInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.EventId == item.Id);
                double ratingAVG = 0;
                double sumRating = 0;

                

                if (evtInFeedback != null)
                {
                    count = evtInFeedback.Count();
                    foreach (var item2 in evtInFeedback)
                    {
                        sumRating = (double)(sumRating + item2.Rating);
                    }
                    if (count != 0)
                    {
                        ratingAVG = sumRating / count;
                    }

                }

                DateTime startDate = (DateTime)item.StartDate;
                DateTime endDate = (DateTime)item.EndDate;
                
                EventFeedbackResponse eventObj = new EventFeedbackResponse()
                {
                    
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Image = item.Image,
                    Rating = Math.Round((float)ratingAVG, 2),                   
                    StartDate = startDate.Date.ToString("dd/MM/yyyy"),
                    EndDate = endDate.Date.ToString("dd/MM/yyyy"),
                    TotalFeedback = count
                    
                };
                if (eventObj.Rating >= highlightRate)
                {
                    listEvent.Add(eventObj);
                }
            }
            return listEvent;
        }

        public async Task<int> UpdateEvent(int id, string Name, string Description, string Image, string Status, DateTime StartDate, DateTime EndDate)
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
                eTourGuide.Data.Entity.ExhibitInEvent evtTrans = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInEvent>().GetAll().Where(x => x.EventId == id).FirstOrDefault();
                if (evtTrans != null)
                {
                    statusToDb = 1;
                }
                
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
                evt.Name = Name;
                evt.Description = Description;
                evt.Image = Image;
                evt.StartDate = StartDate;
                evt.EndDate = EndDate;
                evt.Status = statusToDb;
                await _unitOfWork.CommitAsync();

                return evt.Id;
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Insert Error!!!");
            }
        }

        public async Task<int> UpdateStatusFromWatingToActive(int id)
        {
            var evt = _unitOfWork.Repository<Event>().GetById(id);
            if (evt == null)
            {
                throw new Exception("Cant Not Found This Event!");
            }

            var evtTrans = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInEvent>().GetAll().Where(x => x.EventId == id);
            EventInRoom eventInRoom = _unitOfWork.Repository<EventInRoom>().GetAll().Where(e => e.EventId== id).FirstOrDefault();

            if (eventInRoom == null)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "You must set room for this event to active!!!");
            }else
            {
                if (evtTrans.ToList().Count > 0)
                {

                    try
                    {
                        evt.Status = 2;
                        await _unitOfWork.CommitAsync();
                        return (int)evt.Status;
                    }
                    catch (Exception)
                    {
                        throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Update Error!!!");
                    }
                }
                else
                {
                    throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Update Error!!!");
                }
            }                                
        }
    }
}
