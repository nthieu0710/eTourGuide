using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eTourGuide.Service.Services.ImplService
{
    public class DurationService : IDurationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DurationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public  List<ExhibitFeedbackResponse> DurationForEvent(int id, TimeSpan time)
        {
            var evtTrans = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(x => x.EventId == id);
            evtTrans = evtTrans.OrderByDescending(exhibit => exhibit.PriorityLevel);
            List<Exhibit> listExhibit = new List<Exhibit>();
            if (evtTrans != null)
            {
                foreach (var item in evtTrans)
                {
                    Exhibit exhibit = new Exhibit()
                    {
                        Id = item.Exhibit.Id,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        Image = item.Exhibit.Image,
                        CreateDate = item.Exhibit.CreateDate,
                        Rating = item.Exhibit.Rating,
                        Status = item.Exhibit.Status,
                        Duration = item.Exhibit.Duration
                    };
                    listExhibit.Add(exhibit);
                }
            }

            TimeSpan duration = new TimeSpan(00, 00, 00);
            List<ExhibitFeedbackResponse> listExhibitForDuration = new List<ExhibitFeedbackResponse>();
            if (duration == time)
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

                    ExhibitFeedbackResponse exhibitRes = new ExhibitFeedbackResponse()
                    {
                        Id = item.Exhibit.Id,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        Image = item.Exhibit.Image,
                        Rating = ratingAVG,
                        TotalFeedback = count
                    };
                    listExhibitForDuration.Add(exhibitRes);
                }
                return listExhibitForDuration.ToList();
            }
            /*while (duration <= time)
            {*/


                foreach (var item in listExhibit)
                {
                    duration = (TimeSpan)(duration + item.Duration);
                    if (duration > time)
                    {
                        break;
                    }
                int count = 0;
                var exhibitInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.ExhibittId == item.Id);
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

                ExhibitFeedbackResponse exhibitRes = new ExhibitFeedbackResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Image = item.Image,
                    Rating = ratingAVG,
                    TotalFeedback = count
                };
                listExhibitForDuration.Add(exhibitRes);


            }
            //}
            return listExhibitForDuration;
        }

        public List<ExhibitFeedbackResponse> DurationForTopic(int id, TimeSpan time)
        {
            var evtTrans = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll().Where(x => x.TopicId == id);
            evtTrans = evtTrans.OrderByDescending(exhibit => exhibit.PriorityLevel);
            List<Exhibit> listExhibit = new List<Exhibit>();
            if (evtTrans != null)
            {
                foreach (var item in evtTrans)
                {
                    Exhibit exhibit = new Exhibit()
                    {
                        Id = item.Exhibit.Id,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        Image = item.Exhibit.Image,
                        CreateDate = item.Exhibit.CreateDate,
                        Rating = item.Exhibit.Rating,
                        Status = item.Exhibit.Status,
                        Duration = item.Exhibit.Duration
                    };
                    listExhibit.Add(exhibit);
                }
            }

            TimeSpan duration = new TimeSpan(00, 00, 00);
            List<ExhibitFeedbackResponse> listExhibitForDuration = new List<ExhibitFeedbackResponse>();
            if (duration == time)
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

                    ExhibitFeedbackResponse exhibitRes = new ExhibitFeedbackResponse()
                    {
                        Id = item.Exhibit.Id,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        Image = item.Exhibit.Image,
                        Rating = ratingAVG,
                        TotalFeedback = count
                    };
                    listExhibitForDuration.Add(exhibitRes);
                }
                return listExhibitForDuration.ToList();
            }
            /*while (duration <= time)
            {*/


            foreach (var item in listExhibit)
            {
                duration = (TimeSpan)(duration + item.Duration);
                if (duration > time)
                {
                    break;
                }
                int count = 0;
                var exhibitInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.ExhibittId == item.Id);
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

                ExhibitFeedbackResponse exhibitRes = new ExhibitFeedbackResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Image = item.Image,
                    Rating = ratingAVG,
                    TotalFeedback = count
                };
                listExhibitForDuration.Add(exhibitRes);


            }
            //}
            return listExhibitForDuration;
        }
    }
}
