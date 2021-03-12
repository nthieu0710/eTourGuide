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

        public async Task<int> AddExhibitToTopic(int topicId, int exhibitId)
        {
            int rs = 0;
            TopicInRoom room = _unitOfWork.Repository<TopicInRoom>().GetAll().Where(r => r.TopicId == topicId).FirstOrDefault();
            Topic topic = _unitOfWork.Repository<Topic>().GetById(topicId);
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
            
            ExhibitInTopic exhibitInTopic = new ExhibitInTopic
            {
                ExhibitId = exhibit.Id,
                TopicId = topicId,
                RoomId = roomId,
                CreateDate = dtnew
            };
            
            try
            {
                
                await _unitOfWork.Repository<ExhibitInTopic>().InsertAsync(exhibitInTopic);
                await _unitOfWork.CommitAsync();

                exhibit.Status = 1;
                if (topic.Status == 0)
                {
                    topic.Status = 1;
                    _unitOfWork.Repository<Topic>().Update(topic, topic.Id);
                }
                

               
                _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);
                
                await _unitOfWork.CommitAsync();
                
                rs = 1;
                return rs;
            }
            catch (Exception)
            {
                
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Add Object To Topic Error!!!");
            }   
        }

        //Implement from Interface ITopicService - thêm mới Topic
        public async Task<int> AddTopic(string Name, string Description, string Image, DateTime StartDate)
        {
            int statusToDb = 0;
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);
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
                
                return topic.Id;
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Insert Error!!!");
            }
        }

        public async Task<int> DeleteTopic(int id)
        {
            Topic topic = _unitOfWork.Repository<Topic>().GetById(id);
            if (topic == null)
            {
                throw new Exception("Cant Not Found This Topic!");
            }
            if (topic.Status == 0 || topic.Status == 1 || topic.Status == 3 || topic.Status == 4)
            {
               
                    //xem coi có object nào đang thuộc event muốn xóa hay không
                    var checkExhibitInTopic = _unitOfWork.Repository<ExhibitInTopic>().GetAll().Where(e => e.TopicId == id).AsQueryable();

                    try
                    {
                        if (checkExhibitInTopic.Count() != 0)
                        {
                            _unitOfWork.Repository<ExhibitInTopic>().DeleteRange(checkExhibitInTopic);

                            foreach (var item in checkExhibitInTopic)
                            {
                                Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(item.ExhibitId);



                                //thay đổi status của exhibit thành ready
                                exhibit.Status = 0;
                                _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);
                                //await _unitOfWork.CommitAsync();

                            }

                            TopicInRoom topicInRoom = _unitOfWork.Repository<TopicInRoom>().GetAll().Where(e => e.TopicId == id).FirstOrDefault();

                            Room room = _unitOfWork.Repository<Room>().GetAll().Where(r => r.Id == topicInRoom.RoomId).FirstOrDefault();

                            //xóa row của event trong EventInRoom
                            _unitOfWork.Repository<TopicInRoom>().Delete(topicInRoom);


                            //set status của room thành 0
                            room.Status = 0;
                            _unitOfWork.Repository<Room>().Update(room, room.Id);
                            
                            topic.IsDelete = true;
                            await _unitOfWork.CommitAsync();

                    }


                                    
                }
                catch (Exception)
                {
                    throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not delete topic!!!");
                }
            } else if (topic.Status == 2)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Can not delete topic because this Topic is happening!!!");
            }
            return topic.Id;
        }


        //Implement from Interface ITopicService - Get tất cả Topic
        public List<TopicResponse> GetAllTopics()
        {
            string statusConvert = "";
            var rs = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.IsDelete == false).AsQueryable();
            List<TopicResponse> listTopicResponse = new List<TopicResponse>();
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
            
            var rs = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.Status == 2 && t.IsDelete == false).AsQueryable();
            List<TopicResponseForUser> listTopicResponse = new List<TopicResponseForUser>();
            foreach (var item in rs)
            {
                if (item.Status == 2)
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
            int highlightRate = 4;
            var topic = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.Status == 2 && t.IsDelete == false).AsQueryable();
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
                    if (topicObj.Rating >= highlightRate)
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
                statusConvert = "Waiting";
            }
            else if (topic.Status == 2)
            {
                statusConvert = "Active ";
            }
            else if (topic.Status == 3)
            {
                statusConvert = "Disactive ";
            }
            else if (topic.Status == 4)
            {
                statusConvert = "Closed ";
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

        public List<TopicResponse> GetTopicHasNoRoom()
        {
            string statusConvert = "";
            var rs = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.IsDelete == false).AsQueryable();

            List<TopicResponse> listTopicResponse = new List<TopicResponse>();
            foreach (var item in rs)
            {
                //duyệt trong table TopicInRoom xem topic này đã có phòng hay chưa
                TopicInRoom topicInRoom = _unitOfWork.Repository<TopicInRoom>().GetAll().Where(t => t.TopicId == item.Id).FirstOrDefault();

                //nếu topic chưa có phòng thì add vào list
                if (topicInRoom == null)
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
            }
            return listTopicResponse.ToList();
        }

        public async Task<int> UpdateStatusFromWatingToActive(int id)
        {
            Topic topic = _unitOfWork.Repository<Topic>().GetAll().Where(t =>t.Id == id && t.IsDelete == false && t.Status == 1).FirstOrDefault();
            if (topic == null)
            {
                throw new Exception("Cant Not Found This Topic!");
            }
            
            var topicTrans = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll().Where(x => x.TopicId == topic.Id);

            TopicInRoom topicInRoom = _unitOfWork.Repository<TopicInRoom>().GetAll().Where(t => t.TopicId == id).FirstOrDefault();
            
            if (topicInRoom == null)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "You must set room for this topic to active!!!");
            }else
            {
                if (topicTrans.ToList().Count > 0)
                {
                    try
                    {
                        topic.Status = 2;
                        await _unitOfWork.CommitAsync();
                        return (int)topic.Status;
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

        //Implement from Interface ITopicService - cập nhập Topic
        public async Task<int> UpdateTopic(int id, string Name, string Description, string Image, DateTime StartDate, string Status)
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
            else if (Status == "Waiting")
            {
                eTourGuide.Data.Entity.ExhibitInTopic topicTrans = _unitOfWork.Repository<eTourGuide.Data.Entity.ExhibitInTopic>().GetAll().Where(x => x.TopicId == id).FirstOrDefault();
                if (topicTrans != null)
                {
                    statusToDb = 1;
                }                
            } else if (Status == "Active")
            {
                statusToDb = 2;
            }else if (Status == "Disactive")
            {
                statusToDb = 3;
            }else if (Status == "Closed")
            {
                statusToDb = 4;
            }
           
            try
            {
                topic.Name = Name;
                topic.Description = Description;
                topic.Image = Image;
                topic.StartDate = StartDate;
                topic.Status = statusToDb;
                
                await _unitOfWork.CommitAsync();
               
                return topic.Id;
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Update Error!!!");
            }
        }
    }
}
