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
    public class ExhibitInTopicService : IExhibitInTopicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExhibitInTopicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        public async Task<int> DeleteExhibitIntTopic(int topicId, int exhibitId)
        {
            int rs = 0;
            //lấy obj trong bảng ExhibitInEvent chứa topicId và exhibitId đó
            ExhibitInTopic exhibitInTopic = _unitOfWork.Repository<ExhibitInTopic>().GetAll().Where(e => e.TopicId == topicId && e.ExhibitId == exhibitId).FirstOrDefault();
            if (exhibitInTopic != null)
            {
                try
                {
                    //Delete row trong bảng ExhibitInEvent
                    _unitOfWork.Repository<ExhibitInTopic>().Delete(exhibitInTopic);

                    //Get event đó ra để set status thành Ready
                    Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(exhibitId);
                    exhibit.Status = 0;


                    _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);

                    await _unitOfWork.CommitAsync();

                    rs = 1;
                    return rs;
                }
                catch (Exception)
                {
                    throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not delete exhibit in this topic!!!");
                }
            }
            return rs;
        }

        public List<ExhibitFeedbackResponse> GetExhibitInTopic(int id)
        {
            var topic = _unitOfWork.Repository<Topic>().GetById(id);
            if (topic == null)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not found Topic!!!");
            }

            var topicTrans = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll().Where(x => x.TopicId == id);
            List<ExhibitFeedbackResponse> listExhibit = new List<ExhibitFeedbackResponse>();
            if (topicTrans != null)
            {
                foreach (var item in topicTrans)
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

        public List<ExhibitResponse> GetExhibitInTopicForAdmin(int id)
        {
            var topic = _unitOfWork.Repository<Topic>().GetById(id);
            if (topic == null)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not found Topic!!!");
            }

            var topicTrans = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll().Where(x => x.TopicId == id);
            List<ExhibitResponse> listExhibit = new List<ExhibitResponse>();
            if (topicTrans != null)
            {
                foreach (var item in topicTrans)
                {
                    DateTime createDate = (DateTime)item.Exhibit.CreateDate;

                    listExhibit.Add(new ExhibitResponse
                    {
                        Id = item.Exhibit.Id,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        Image = item.Exhibit.Image,
                        CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                        Rating = item.Exhibit.Rating,
                        Status = "Added",
                        Duration = (TimeSpan)item.Exhibit.Duration
                    });
                }
            }
            return listExhibit;
        }
    }
}
