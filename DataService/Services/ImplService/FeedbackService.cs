using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
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

     
    }
}
