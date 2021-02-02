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
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public TopicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;          
        }

        //Implement from Interface ITopicService - thêm mới Topic
        public async Task<Topic> AddTopic(string Name, string Description, string Image, DateTime StartDate)
        {
            int statusToDb = 0;
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);
            /*if (Status == "New")
            {
                statusToDb = 0;
            }
            else if (Status == "Ready")
            {
                statusToDb = 1;
            }
            else if (Status == "Closed")
            {
                statusToDb = 2;
            }*/
            Topic topic = new Topic
            {
                Name = Name,
                Description = Description,
                Image = Image,
                CreateDate = dtnew,
                StartDate = StartDate,
                Rating = 0,
                Status = statusToDb,
                IsDelete = false
            };        
            try
            {
                await _unitOfWork.Repository<Topic>().InsertAsync(topic);
                await _unitOfWork.CommitAsync();
                
                return topic;
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Insert Error!!!");
            }
        }

        public async Task<Topic> DeleteTopic(int id)
        {
            Topic topic = _unitOfWork.Repository<Topic>().GetById(id);
            if (topic == null)
            {
                throw new Exception("Cant Not Found This Topic!");
            }
            if (topic.Status == 0 || topic.Status == 2)
            {
                try
                {
                    topic.IsDelete = true;
                    await _unitOfWork.CommitAsync();             
                }
                catch (Exception)
                {
                    throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not delete topic!!!");
                }
            } else if (topic.Status == 1)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not delete topic!!!");
            }
            return topic;
        }


        //Implement from Interface ITopicService - Get tất cả Topic
        public List<TopicResponse> GetAllTopics()
        {
            string statusConvert = "";
            var rs = _unitOfWork.Repository<Topic>().GetAll().AsQueryable();
            List<TopicResponse> listTopicResponse = new List<TopicResponse>();
            foreach (var item in rs)
            {
                if (item.Status == 0)
                {
                    statusConvert = "New";
                }
                else if (item.Status == 1)
                {
                    statusConvert = "Ready";
                }
                else if (item.Status == 2)
                {
                    statusConvert = "Closed";
                }
                if (item.IsDelete == false)
                {
                    DateTime createDate = (DateTime)item.CreateDate;
                    DateTime startDate = (DateTime)item.StartDate;
                    TopicResponse topicResponse = new TopicResponse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Image = item.Image,
                        CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                        StartDate = startDate.Date.ToString("yyyy-MM-dd"),
                        Rating = (float)item.Rating,
                        Status = statusConvert,
                        isDelete = (bool)item.IsDelete
                    };
                    listTopicResponse.Add(topicResponse);
                }
               
            }
            return listTopicResponse.ToList();
        }

        public List<TopicResponseForUser> GetAllTopicsForUser()
        {
            
            var rs = _unitOfWork.Repository<Topic>().GetAll().AsQueryable();
            List<TopicResponseForUser> listTopicResponse = new List<TopicResponseForUser>();
            foreach (var item in rs)
            {
                if (item.Status == 1)
                {
                    TopicResponseForUser topicResponse = new TopicResponseForUser()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Image = item.Image,
                        StartDate = (DateTime)item.StartDate,
                        Rating = (float)item.Rating,
                    };
                    listTopicResponse.Add(topicResponse);
                }
                
            }
            return listTopicResponse.ToList();
        }


        //Get highlight topic with rating > 4
        public List<TopicFeedbackResponse> GetHightLightTopic()
        {
            
            var topic = _unitOfWork.Repository<Topic>().GetAll().AsQueryable();
            List<TopicFeedbackResponse> listTopic = new List<TopicFeedbackResponse>();

            foreach (var item in topic)
            {
                int count = 0;
                var topicInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.TopicId == item.Id);
                double rating = 0;
                double sumRating = 0;

                DateTime dt = (DateTime)item.StartDate;
                if (topicInFeedback != null)
                {
                    count = topicInFeedback.Count();
                    foreach (var item2 in topicInFeedback)
                    {
                        sumRating = (double)(sumRating + item2.Rating);
                    }
                    if (count != 0)
                    {
                        rating = sumRating / count;
                    }

                }
                TopicFeedbackResponse topicObj = new TopicFeedbackResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Image = item.Image,                   
                    StartDate = dt.Date.ToString("dd/MM/yyyy"),
                    Rating = rating,
                    TotalFeedback = count
                   
                };
                if (topicObj.Rating >= 4)
                {
                    listTopic.Add(topicObj);
                }
            }
            return listTopic;
        }

        public async Task<TopicResponse> GetTopicById(int id)
        {
            var topic = _unitOfWork.Repository<Topic>().GetById(id);
            if (topic == null)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not find Topic!!!");
            }
            string statusConvert = "";
            if (topic.Status == 0)
            {
                statusConvert = "New";
            }
            else if (topic.Status == 1)
            {
                statusConvert = "Ready";
            }
            else if (topic.Status == 2)
            {
                statusConvert = "Closed";
            }
            DateTime createDate = (DateTime)topic.CreateDate;
            DateTime startDate = (DateTime)topic.StartDate;
            TopicResponse topicResponse = new TopicResponse();
            topicResponse.Id = topic.Id;
            topicResponse.Name = topic.Name;
            topicResponse.Description = topic.Description;
            topicResponse.Image = topic.Image;
            topicResponse.CreateDate = createDate.Date.ToString("yyyy-MM-dd");
            topicResponse.StartDate = startDate.Date.ToString("yyyy-MM-dd");
            topicResponse.Rating = (float)topic.Rating;
            topicResponse.Status = statusConvert;
            topicResponse.isDelete = (bool)topic.IsDelete;
            return topicResponse;
        }

        //Implement from Interface ITopicService - cập nhập Topic
        public async Task<Topic> UpdateTopic(int id, string Name, string Description, string Image, DateTime StartDate, string Status)
        {
            int statusToDb = 0;
            Topic topic = _unitOfWork.Repository<Topic>().GetById(id);
            if (topic == null)
            {
                throw new Exception("Cant Not Found This Topic!");
            }
            
            if (Status == "New")
            {
                statusToDb = 0;
            }
            else if (Status == "Ready")
            {
                statusToDb = 1;
            } else if (Status == "Closed")
            {
                statusToDb = 2;
            }
            topic.Name = Name;
            topic.Description = Description;
            topic.Image = Image;
            topic.StartDate = StartDate;
            //topic.Rating = Rating;
            topic.Status = statusToDb;
            try
            {
                
                await _unitOfWork.CommitAsync();
               
                return topic;
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Insert Error!!!");
            }
        }
    }
}
