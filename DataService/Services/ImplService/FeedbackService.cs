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
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeedbackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        public List<Feedback> GetFeedbacksEventForUserById(int Id)
        {
            var eventFeedbacks = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.EventId == Id);
            List<Feedback> listFeedback = new List<Feedback>();
            if (eventFeedbacks != null)
            {
                foreach (var item in eventFeedbacks)
                {
                    Feedback feedback = new Feedback()
                    {
                        Id = item.Id,
                        EventId = item.EventId,
                        VisitorName = item.VisitorName,
                        Rating = item.Rating,
                        Description = item.Description,
                        DateTime = item.DateTime
                    };
                    listFeedback.Add(feedback);
                }
            }
            return listFeedback;
        }

        public List<EventFeedbackFromUser> GetFeedbacksEventForAdmin()
        {
            var feedbacks = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.EventId != null);
            List<EventFeedbackFromUser> listRs = new List<EventFeedbackFromUser>();
            foreach (var item in feedbacks)
            {
                var evt = _unitOfWork.Repository<Event>().GetAll().Where(x => x.Id == item.EventId).FirstOrDefault();
                DateTime createDate = (DateTime)item.DateTime;
                EventFeedbackFromUser rs = new EventFeedbackFromUser
                {
                    Id = item.Id,
                    EventId = (int)item.EventId,
                    EventName = evt.Name,
                    VisitorName = item.VisitorName,
                    Rating = (double)item.Rating,
                    Description = item.Description,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd")
                };
                listRs.Add(rs);
            }
            return listRs;
        }

        public List<Feedback> GetFeedbacksExhibitcForUserById(int Id)
        {
            var exhibitFeedbacks = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.ExhibittId == Id);
            List<Feedback> listFeedback = new List<Feedback>();
            if (exhibitFeedbacks != null)
            {
                foreach (var item in exhibitFeedbacks)
                {
                    Feedback feedback = new Feedback()
                    {
                        Id = item.Id,
                        ExhibittId = item.ExhibittId,
                        VisitorName = item.VisitorName,
                        Rating = item.Rating,
                        Description = item.Description,
                        DateTime = item.DateTime
                    };
                    listFeedback.Add(feedback);
                }
            }
            return listFeedback;
            
        }

        public List<Feedback> GetFeedbacksTopicForUserById(int Id)
        {
            var topicFeedbacks = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.TopicId == Id);
            List<Feedback> listFeedback = new List<Feedback>();
            if (topicFeedbacks != null)
            {
                foreach (var item in topicFeedbacks)
                {
                    Feedback feedback = new Feedback()
                    {
                        Id = item.Id,
                        TopicId = item.TopicId,
                        VisitorName = item.VisitorName,
                        Rating = item.Rating,
                        Description = item.Description,
                        DateTime = item.DateTime
                    };
                    listFeedback.Add(feedback);
                }
            }
            return listFeedback;
        }

        public List<ExhibitFeedbackFromUser> GetFeedbacksExhibitForAdmin()
        {
            var feedbacks = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.ExhibittId != null);
            List<ExhibitFeedbackFromUser> listRs = new List<ExhibitFeedbackFromUser>();
            foreach (var item in feedbacks)
            {
                var exhibit = _unitOfWork.Repository<Exhibit>().GetAll().Where(x => x.Id == item.ExhibittId).FirstOrDefault();
                DateTime createDate = (DateTime)item.DateTime;
                ExhibitFeedbackFromUser rs = new ExhibitFeedbackFromUser
                {
                    Id = item.Id,
                    ExhibitId = (int)item.ExhibittId,
                    ExhibitName = exhibit.Name,
                    VisitorName = item.VisitorName,
                    Rating = (double)item.Rating,
                    Description = item.Description,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd")
                };
                listRs.Add(rs);
            }
            return listRs;
        }

        public List<TopicFeedbackFromUser> GetFeedbacksTopicForAdmin()
        {
            var feedbacks = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.TopicId != null);
            List<TopicFeedbackFromUser> listRs = new List<TopicFeedbackFromUser>();
            foreach (var item in feedbacks)
            {
                var topic = _unitOfWork.Repository<Topic>().GetAll().Where(x => x.Id == item.TopicId).FirstOrDefault();
                DateTime createDate = (DateTime)item.DateTime;
                TopicFeedbackFromUser rs = new TopicFeedbackFromUser
                {
                    Id = item.Id,
                    TopicId = (int)item.TopicId,
                    TopicName = topic.Name,
                    VisitorName = item.VisitorName,
                    Rating = (double)item.Rating,
                    Description = item.Description,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd")
                };
                listRs.Add(rs);
            }
            return listRs;
        }



        public async Task<int> CreateUserFeedbackForExhibit(int exhibitId, string visitorName, double rating, string description)
        {
            int rs = 0;
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);

            //tạo object feedback
            Feedback exhibitFeedback = new Feedback
            {
                ExhibittId = exhibitId,
                VisitorName = visitorName,
                Rating = rating,
                Description = description,
                DateTime = dtnew
            };

            //tính toán rating trung bình 
            int count = 0;
            var exhibitInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.ExhibittId == exhibitId);
            double ratingAVGForExhibit = 0;
            double sumRating = 0;

            
            if (exhibitInFeedback != null)
            {
                count = exhibitInFeedback.Count();
                foreach (var item in exhibitInFeedback)
                {
                    sumRating = (double)(sumRating + item.Rating);
                }
                if (count != 0)
                {
                    ratingAVGForExhibit = (sumRating + rating) / (count + 1);
                }
                else
                {
                    ratingAVGForExhibit = rating;
                }

            }

            //get exhibit ra để set rating field
            Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(exhibitId);


            try
            {


                await _unitOfWork.Repository<Feedback>().InsertAsync(exhibitFeedback);
                await _unitOfWork.CommitAsync();

                exhibit.Rating = Math.Round(ratingAVGForExhibit, 2);
                _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);
                await _unitOfWork.CommitAsync();

                rs = 1;
                return rs;
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Insert Feedback Error!!!");
            }
        }

        public async Task<int> CreateUserFeedbackForEvent(int eventId, string visitorName, double rating, string description)
        {
            int rs = 0;
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);

            //tạo object feedback
            Feedback eventFeedback = new Feedback
            {
                EventId = eventId,
                VisitorName = visitorName,
                Rating = rating,
                Description = description,
                DateTime = dtnew
            };

            //tính toán rating trung bình 
            int count = 0;
            var eventInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.EventId == eventId);
            double ratingAVGForExhibit = 0;
            double sumRating = 0;


            if (eventInFeedback != null)
            {
                count = eventInFeedback.Count();
                foreach (var item in eventInFeedback)
                {
                    sumRating = (double)(sumRating + item.Rating);
                }
                if (count != 0)
                {
                    ratingAVGForExhibit = (sumRating + rating) / (count + 1);
                }else
                {
                    ratingAVGForExhibit = rating;
                }

            }

            //get event ra để set rating field
            Event evt = _unitOfWork.Repository<Event>().GetById(eventId);


            try
            {


                await _unitOfWork.Repository<Feedback>().InsertAsync(eventFeedback);
                await _unitOfWork.CommitAsync();

                evt.Rating = Math.Round(ratingAVGForExhibit, 2);
                _unitOfWork.Repository<Event>().Update(evt, evt.Id);
                await _unitOfWork.CommitAsync();

                rs = 1;
                return rs;
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Insert Feedback Error!!!");
            }
        }

        public async Task<int> CreateUserFeedbackForTopic(int topicId, string visitorName, double rating, string description)
        {
            int rs = 0;
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);

            //tạo object feedback
            Feedback topicFeedback = new Feedback
            {
                TopicId = topicId,
                VisitorName = visitorName,
                Rating = rating,
                Description = description,
                DateTime = dtnew
            };

            //tính toán rating trung bình 
            int count = 0;
            var topicInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.TopicId == topicId);
            double ratingAVGForExhibit = 0;
            double sumRating = 0;


            if (topicInFeedback != null)
            {
                count = topicInFeedback.Count();
                foreach (var item in topicInFeedback)
                {
                    sumRating = (double)(sumRating + item.Rating);
                }
                if (count != 0)
                {
                    ratingAVGForExhibit = (sumRating + rating) / (count + 1);
                }
                else
                {
                    ratingAVGForExhibit = rating;
                }

            }

            //get exhibit ra để set rating field
            Topic topic = _unitOfWork.Repository<Topic>().GetById(topicId);

            //cộng vs cả rating của user vừa nhập vào để ra rating cuối cùng
            

            try
            {


                await _unitOfWork.Repository<Feedback>().InsertAsync(topicFeedback);
                await _unitOfWork.CommitAsync();

                topic.Rating = Math.Round(ratingAVGForExhibit, 2);
                _unitOfWork.Repository<Topic>().Update(topic, topic.Id);
                await _unitOfWork.CommitAsync();

                rs = 1;
                return rs;
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Insert Feedback Error!!!");
            }
        }
    }
}
