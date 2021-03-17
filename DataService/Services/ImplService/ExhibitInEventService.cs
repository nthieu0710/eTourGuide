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
    public class ExhibitInEventService : IExhibitInEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExhibitInEventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> DeleteExhibitInEvent(int exhibitId)
        {
            int rs = 0;
            //lấy obj trong bảng ExhibitInEvent chứa eventId và exhibitId đó
            ExhibitInEvent exhibitInEvent = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(e => e.ExhibitId == exhibitId).FirstOrDefault();
            Event evt = _unitOfWork.Repository<Event>().GetAll().Where(e => e.Id == exhibitInEvent.EventId).FirstOrDefault();
            if (exhibitInEvent != null)
            {
                try
                {
                    //Delete row trong bảng ExhibitInEvent
                    _unitOfWork.Repository<ExhibitInEvent>().Delete(exhibitInEvent);

                    //Get exhibit đó ra để set status thành Ready
                    Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(exhibitId);
                    exhibit.Status = 0;


                    _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);
                    await _unitOfWork.CommitAsync();


                    var exhibitInEventList = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(e => e.EventId == evt.Id).AsQueryable();
                    if (exhibitInEventList.ToList().Count == 0)
                    {
                        evt.Status = 0;
                        _unitOfWork.Repository<Event>().Update(evt, evt.Id);
                    }

                    await _unitOfWork.CommitAsync();

                    rs = 1;
                    return rs;
                }
                catch (Exception)
                {
                    throw new Exception("Can not delete exhibit in this event!!!");
                }
            }
            return rs;
        }

        public List<ExhibitResponse> GetExhibitInEvent(int id)
        {
            var evt = _unitOfWork.Repository<Event>().GetById(id);
            if (evt == null)
            {
                throw new Exception("Can not found Event!!!");
            }

            var evtTrans = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(x => x.EventId == id);
            List<ExhibitResponse> listExhibit = new List<ExhibitResponse>();
            if (evtTrans != null)
            {
                foreach (var item in evtTrans)
                {
                    int count = 0;
                    var exhibitInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.ExhibittId == item.Exhibit.Id);
                   
                    if (exhibitInFeedback != null)
                    {
                        count = exhibitInFeedback.Count();
                       

                    }

                    ExhibitResponse obj = new ExhibitResponse()
                    {
                        Id = item.Exhibit.Id,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        Image = item.Exhibit.Image,
                        Rating = (double)item.Exhibit.Rating,
                        TotalFeedback = count
                    };
                    listExhibit.Add(obj);
                }
            }
            return listExhibit;
           
        }

        public List<ExhibitResponse> GetExhibitInEventForAdmin(int id)
        {
            var evt = _unitOfWork.Repository<Event>().GetById(id);
            if (evt == null)
            {
                throw new Exception("Can not found Event!!!");
            }

            var evtTrans = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInEvent>().GetAll().Where(x => x.EventId == id);
            List<ExhibitResponse> listExhibit = new List<ExhibitResponse>();
            if (evtTrans != null)
            {
                foreach (var item in evtTrans)
                {
                    DateTime createDate = (DateTime)item.Exhibit.CreateDate;

                    listExhibit.Add(new ExhibitResponse
                    {
                        Id = item.Exhibit.Id,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        Image = item.Exhibit.Image,
                        CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                        Rating = (double)item.Exhibit.Rating,
                        Status = "Added",
                        Duration = (TimeSpan)item.Exhibit.Duration
                    });
                }
            }
            return listExhibit;
        }
    }
}
