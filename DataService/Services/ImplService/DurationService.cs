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
        public List<ExhibitFeedbackResponse> SuggestExhibitFromDuration(TimeSpan time)
        {
            var rs = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Status == 1).AsQueryable();
            rs = rs.OrderByDescending(exhibit => exhibit.Rating);
            TimeSpan duration = new TimeSpan(00, 00, 00);
            List<ExhibitFeedbackResponse> listExhibitForDuration = new List<ExhibitFeedbackResponse>();
            foreach (var item in rs)
            {
                int count = 0;
                var exhibitInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.ExhibittId == item.Id);            
                if (exhibitInFeedback != null)
                {
                    count = exhibitInFeedback.Count();                  
                }
                ExhibitFeedbackResponse exhibitRes = new ExhibitFeedbackResponse()
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
                listExhibitForDuration = new List<ExhibitFeedbackResponse>();
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
                    ExhibitFeedbackResponse exhibitRes = new ExhibitFeedbackResponse()
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








        /*public  List<ExhibitFeedbackResponse> DurationForEvent(int id, TimeSpan time)
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
            *//*while (duration <= time)
            {*//*


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
            *//*while (duration <= time)
            {*//*


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
        }*/

        
    }
}
