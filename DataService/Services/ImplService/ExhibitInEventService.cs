using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using eTourGuide.Service.Exceptions;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eTourGuide.Service.Services.ImplService
{
    public class ExhibitInEventService : IExhibitInEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExhibitInEventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
      

        List<ExhibitFeedbackResponse> IExhibitInEventService.GetExhibitInEvent(int id)
        {
            var evt = _unitOfWork.Repository<Event>().GetById(id);
            if (evt == null)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not found Event!!!");
            }

            var evtTrans = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(x => x.EventId == id);
            List<ExhibitFeedbackResponse> listExhibit = new List<ExhibitFeedbackResponse>();
            if (evtTrans != null)
            {
                foreach (var item in evtTrans)
                {
                    int count = 0;
                    var exhibitInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.ExhibittId == item.Exhibit.Id);
                    double ratingAVG = 0;
                    double sumRating = 0;
                    if (exhibitInFeedback != null)
                    {
                        count = exhibitInFeedback.Count();
                        foreach (var item2 in exhibitInFeedback)
                        {
                            sumRating = (double)(sumRating + item2.Rating);
                        }
                        if (count != 0)
                        {
                            ratingAVG = sumRating / count;
                        }

                    }

                    ExhibitFeedbackResponse obj = new ExhibitFeedbackResponse()
                    {
                        Id = item.Exhibit.Id,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        Image = item.Exhibit.Image,
                        Rating = ratingAVG,
                        TotalFeedback = count
                    };
                    listExhibit.Add(obj);
                }
            }
            return listExhibit;
           
        }
    }
}
