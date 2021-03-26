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

        

        public async Task<int> DeleteExhibitIntTopic(int exhibitId)
        {
            int rs = 0;
            //lấy obj trong bảng exhibitInTopic chứa topicId và exhibitId đó
            ExhibitInTopic exhibitInTopic = _unitOfWork.Repository<ExhibitInTopic>().GetAll()
                                            .Where(e => e.Status == true && e.ExhibitId == exhibitId).FirstOrDefault();                              

            if (exhibitInTopic != null)
            {
                //lấy topic ra
                Topic topic = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.Id == exhibitInTopic.TopicId).FirstOrDefault();
                try
                {
                    //Delete row trong bảng ExhibitInTopic
                    _unitOfWork.Repository<ExhibitInTopic>().Delete(exhibitInTopic);

                    //Get exhibit đó ra để set status thành Ready
                    Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(exhibitId);
                    exhibit.Status = 0;


                    _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);
                    await _unitOfWork.CommitAsync();


                    //xét trong bảng exhibitInTopic 
                    var exhibitInTopicList = _unitOfWork.Repository<ExhibitInTopic>().GetAll()
                                            .Where(t => t.Status == true && t.TopicId == topic.Id).AsQueryable();

                    //nếu k còn obj nào trong topic đó thì set status cho topic đó về New
                    if (exhibitInTopicList.ToList().Count == 0)
                    {
                        topic.Status = 0;
                        _unitOfWork.Repository<Topic>().Update(topic, topic.Id);
                    }


                    await _unitOfWork.CommitAsync();

                    rs = 1;
                    return rs;
                }
                catch (Exception)
                {
                    throw new Exception("Can not delete exhibit in this topic!!!");
                }
            }
            return rs;
        }

        public List<ExhibitResponse> GetExhibitInTopic(int id)
        {
            var topic = _unitOfWork.Repository<Topic>().GetById(id);
            if (topic == null)
            {
                throw new Exception("Can not found Topic!!!");
            }

            var topicTrans = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll()
                                                    .Where(x => x.Status == true && x.TopicId == id);

            List<ExhibitResponse> listExhibit = new List<ExhibitResponse>();
            if (topicTrans.Count() > 0)
            {
                foreach (var item in topicTrans)
                {                 
                    ExhibitResponse obj = new ExhibitResponse()
                    {
                        Id = item.Exhibit.Id,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        NameEng = item.Exhibit.NameEng,
                        DescriptionEng = item.Exhibit.Description,
                        Image = item.Exhibit.Image,
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
                throw new Exception("Can not found Topic!!!");
            }

            var topicTrans = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll()
                                .Where(x => x.Status == true && x.TopicId == id);

            List<ExhibitResponse> listExhibit = new List<ExhibitResponse>();
            if (topicTrans.Count() > 0)
            {
                foreach (var item in topicTrans)
                {
                    DateTime createDate = (DateTime)item.Exhibit.CreateDate;

                    listExhibit.Add(new ExhibitResponse
                    {
                        Id = item.Exhibit.Id,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        NameEng = item.Exhibit.NameEng,
                        DescriptionEng = item.Exhibit.Description,
                        Image = item.Exhibit.Image,
                        CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                        Status = "Added",
                        Duration = (TimeSpan)item.Exhibit.Duration
                    });
                }
            }
            return listExhibit;
        }

        public List<ExhibitResponse> GetExhbitForClosedTopic(int topicId)
        {
            var exhibitInTopic = _unitOfWork.Repository<ExhibitInTopic>().GetAll()
                                .Where(e => e.Status == false && e.TopicId == topicId).AsQueryable();

            List<ExhibitResponse> listRs = new List<ExhibitResponse>();
            string statusConvert = "";

            if (exhibitInTopic.Count() != 0)
            {
                foreach (var item in exhibitInTopic)
                {
                    if (item.Exhibit.Status == 0)
                    {
                        statusConvert = "Ready";
                    }
                    else if (item.Exhibit.Status == 1)
                    {
                        statusConvert = "Added";
                    }

                    DateTime createDate = (DateTime)item.Exhibit.CreateDate;

                    ExhibitResponse exhibitResponse = new ExhibitResponse()
                    {
                        Id = item.Exhibit.Id,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        NameEng = item.Exhibit.NameEng,
                        DescriptionEng = item.Exhibit.Description,
                        Image = item.Exhibit.Image,
                        CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                        Status = statusConvert,
                        Duration = (TimeSpan)item.Exhibit.Duration,
                        isDelete = (bool)item.Exhibit.IsDelete
                    };
                    listRs.Add(exhibitResponse);
                }

            }
            return listRs.ToList();
        }


    }
}
