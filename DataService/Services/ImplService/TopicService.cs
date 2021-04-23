using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using eTourGuide.Service.Exceptions;
using eTourGuide.Service.Helpers;
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
            
            Topic topic = _unitOfWork.Repository<Topic>().GetById(topicId);
            
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);

            Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(exhibitId);

 
            ExhibitInTopic exhibitInTopic = new ExhibitInTopic
            {
                ExhibitId = exhibit.Id,
                TopicId = topicId,               
                CreateDate = dtnew,
                Status = true
            };
            
            try
            {
                
                await _unitOfWork.Repository<ExhibitInTopic>().InsertAsync(exhibitInTopic);
                await _unitOfWork.CommitAsync();

                exhibit.Status = (int) ExhibitsStatus.Status.Added;
                _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);

                if (topic.Status == (int)TopicStatus.Status.New)
                {
                    topic.Status = (int)TopicStatus.Status.Waiting;
                    _unitOfWork.Repository<Topic>().Update(topic, topic.Id);
                }
                               
                await _unitOfWork.CommitAsync();
                
                rs = 1;
                return rs;
            }
            catch (Exception)
            {                
                throw new Exception("Add Exhibit To Topic Error!!!");
            }   
        }

        //Implement from Interface ITopicService - thêm mới Topic
        public async Task<int> AddTopic(string Name, string Description, string NameEng, string DescriptionEng, string Image, DateTime StartDate, string Username)
        {
            int statusToDb = (int)TopicStatus.Status.New;
            int closedStatus = (int)TopicStatus.Status.Closed;
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);

            var listTopic = _unitOfWork.Repository<Topic>().GetAll().Where(t =>
                                                                        (t.Status != closedStatus || t.IsDelete != true) 
                                                                        &&  (t.Name.Equals(Name) || t.NameEng.Equals(NameEng))
                                                                        ).ToList();
            if (listTopic.Count() > 0)
            {
                throw new Exception("Tên tiếng Việt hoặc tên tiếng Anh của chủ đề đã bị trùng với chủ đề khác!!!");
            }

            Topic topic = new Topic
            {
                Name = Name,
                Description = Description,
                NameEng = NameEng,
                DescriptionEng = DescriptionEng,
                Image = Image,
                CreateDate = dtnew,
                StartDate = StartDate,
                Rating = 0,
                Status = statusToDb,
                IsDelete = false,
                RoomId = null, 
                Username = Username
            };        
            try
            {
                await _unitOfWork.Repository<Topic>().InsertAsync(topic);
                await _unitOfWork.CommitAsync();                
                return topic.Id;
            }
            catch (Exception)
            {
                throw new Exception("Insert Error!!!");
            }
        }

        public async Task<int> DeleteTopic(int id)
        {
            Topic topic = _unitOfWork.Repository<Topic>().GetById(id);
            if (topic == null)
            {
                throw new Exception("Cant Not Found This Topic!");
            }
            if (topic.Status == (int)TopicStatus.Status.New || topic.Status == (int)TopicStatus.Status.Waiting || topic.Status == (int)TopicStatus.Status.Disactive || topic.Status == (int)TopicStatus.Status.Closed)
            {
               
                //xem coi có exhibit nào đang thuộc topic muốn xóa hay không
                var checkExhibitInTopic = _unitOfWork.Repository<ExhibitInTopic>().GetAll().Where(e => e.TopicId == id).AsQueryable();
                var checkFeedbackTopic = _unitOfWork.Repository<Feedback>().GetAll().Where(f => f.TopicId == id).AsQueryable();

                    try
                    {
                        //nếu có data trong bảng exhbit in topic
                        if (checkExhibitInTopic.Count() > 0)
                        {
                            //xóa data trong bảng exhibit in topic
                            _unitOfWork.Repository<ExhibitInTopic>().DeleteRange(checkExhibitInTopic);

                            foreach (var item in checkExhibitInTopic)
                            {
                                Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(item.ExhibitId);
                                //thay đổi status của exhibit thành ready
                                exhibit.Status = (int) ExhibitsStatus.Status.Ready;
                                _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);
                                //await _unitOfWork.CommitAsync();
                            }                                                    
                                             
                        }

                        //nếu có data trong bảng feedback
                        if (checkFeedbackTopic.Count() > 0)
                        {
                            //xóa data của topic trong bảng feedback
                            _unitOfWork.Repository<Feedback>().DeleteRange(checkFeedbackTopic);
                        }


                    if (topic.RoomId != null)
                    {
                        Room room = _unitOfWork.Repository<Room>().GetById((int)topic.RoomId);
                        room.Status = (int)RoomStatus.Status.Ready;
                        //cập nhập room
                        _unitOfWork.Repository<Room>().Update(room, room.Id);
                    }
                    
                            
                        
                    
                        //set isDelete = true để xóa topic
                        topic.IsDelete = true;
                        await _unitOfWork.CommitAsync();

                    }
                    catch (Exception e)
                    {
                        throw new Exception("Can not delete topic!!!");
                    }
            } 
            else if (topic.Status == (int)TopicStatus.Status.Active)
            {
                throw new Exception("Can not delete topic because this Topic is happening!!!");
            }
            return topic.Id;
        }


        //Implement from Interface ITopicService - Get tất cả Topic for admin
        public List<TopicResponse> GetAllTopics()
        {
            string statusConvert = "";
            var rs = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.IsDelete == false).AsQueryable();
            List<TopicResponse> listTopicResponse = new List<TopicResponse>();
            foreach (var item in rs)
            {
                if (item.Status == (int)TopicStatus.Status.New)
                {
                    statusConvert = "Mới";
                }
                else if (item.Status == (int)TopicStatus.Status.Waiting)
                {
                    statusConvert = "Đang chờ kích hoạt";
                }
                else if (item.Status == (int)TopicStatus.Status.Active)
                {
                    statusConvert = "Đang diễn ra";
                }
                else if (item.Status == (int)TopicStatus.Status.Disactive)
                {
                    statusConvert = "Tạm dừng";
                }
                else if (item.Status == (int)TopicStatus.Status.Closed)
                {
                    statusConvert = "Đã đóng";
                }

                string topicInRoom = "Chủ đề này chưa được thiết lập phòng";
                if (item.RoomId != null)
                {
                    topicInRoom = "Chủ đề này đang ở phòng: " + item.Room.No;
                }
                

                    DateTime createDate = (DateTime)item.CreateDate;
                    DateTime startDate = (DateTime)item.StartDate;
                    TopicResponse topicResponse = new TopicResponse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        NameEng = item.NameEng,
                        DescriptionEng = item.DescriptionEng,
                        Image = item.Image,
                        CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                        StartDate = startDate.Date.ToString("yyyy-MM-dd"),
                        Rating = (float)item.Rating,
                        Status = statusConvert,
                        isDelete = (bool)item.IsDelete,
                        RoomNo = topicInRoom
                    };
                    listTopicResponse.Add(topicResponse);     
            }
            return listTopicResponse.ToList();
        }

        public List<TopicResponse> GetAllTopicsForUser()
        {
            
            var rs = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.Status == (int)TopicStatus.Status.Active 
                                                                         && t.IsDelete == false
                                                                         && DateTime.Now >= t.StartDate).AsQueryable();

            List<TopicResponse> listTopicResponse = new List<TopicResponse>();
            foreach (var item in rs)
            {               
                    int count = 0;
                    var evtInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.TopicId == item.Id);
                    if (evtInFeedback.Count() > 0)
                    {
                        count = evtInFeedback.Count();
                    }

                    DateTime startDate = (DateTime)item.StartDate;
                    

                    TopicResponse topicResponse = new TopicResponse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        NameEng = item.NameEng,
                        DescriptionEng = item.DescriptionEng,
                        Image = item.Image,
                        StartDate = startDate.Date.ToString("dd/MM/yyyy"),
                        Rating = (float)item.Rating,
                        TotalFeedback = count
                    };
                    listTopicResponse.Add(topicResponse);          
            }                      
            return listTopicResponse.ToList();
        }


        public List<TopicResponse> SearchTopicForAdmin(string name)
        {
            string statusConvert = "";

            List<TopicResponse> listTopicResponse = new List<TopicResponse>();

            //nếu k nhập gì trả về all
            if (String.IsNullOrEmpty(name))
            {
                listTopicResponse = GetAllTopics();
                return listTopicResponse;
            }


            var topic = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.Name.Contains(name) && t.IsDelete == false).AsQueryable();
            
            foreach (var item in topic)
            {
                if (item.Status == (int)TopicStatus.Status.New)
                {
                    statusConvert = "Mới";
                }
                else if (item.Status == (int)TopicStatus.Status.Waiting)
                {
                    statusConvert = "Đang chờ kích hoạt";
                }
                else if (item.Status == (int)TopicStatus.Status.Active)
                {
                    statusConvert = "Đang diễn ra";
                }
                else if (item.Status == (int)TopicStatus.Status.Disactive)
                {
                    statusConvert = "Tạm dừng";
                }
                else if (item.Status == (int)TopicStatus.Status.Closed)
                {
                    statusConvert = "Đã đóng";
                }

                string topicInRoom = "Chủ đề này chưa được thiết lập phòng";
                if (item.RoomId != null)
                {
                    topicInRoom = "Chủ đề này đang ở phòng: " + item.RoomId;
                }


                    DateTime createDate = (DateTime)item.CreateDate;
                    DateTime startDate = (DateTime)item.StartDate;

                    TopicResponse topicResponse = new TopicResponse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        NameEng = item.NameEng,
                        DescriptionEng = item.DescriptionEng,
                        Image = item.Image,
                        CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                        StartDate = startDate.Date.ToString("yyyy-MM-dd"),
                        Rating = (float)item.Rating,
                        Status = statusConvert,
                        isDelete = (bool)item.IsDelete,
                        RoomNo = topicInRoom
                    };
                    listTopicResponse.Add(topicResponse);
                
            }
            return listTopicResponse.ToList();
        }

        //sort by rating
        public List<TopicResponse> GetHightLightTopic()
        {
            //int highlightRate = 4;

            var topic = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.Status == (int)TopicStatus.Status.Active
                                                                            && t.IsDelete == false
                                                                            && t.Rating >= 4
                                                                            && DateTime.Now >= t.StartDate).AsQueryable();

            List<TopicResponse> listTopic = new List<TopicResponse>();

            foreach (var item in topic)
            {
                int count = 0;
                var topicInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.TopicId == item.Id);
                
                DateTime dt = (DateTime)item.StartDate;

                if (topicInFeedback.Count() > 0)
                {
                    count = topicInFeedback.Count();                    
                }

                TopicResponse topicObj = new TopicResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    NameEng = item.NameEng,
                    DescriptionEng = item.DescriptionEng,
                    Image = item.Image,
                    StartDate = dt.Date.ToString("dd/MM/yyyy"),
                    Rating = (float)item.Rating,
                    TotalFeedback = count
                };
                /*if (topicObj.Rating >= highlightRate)
                {*/
                    listTopic.Add(topicObj);
                /*}*/
     
            }
            listTopic = listTopic.OrderByDescending(response => response.Rating).ToList();
            return listTopic.ToList();
        }


        public async Task<TopicResponse> GetTopicById(int id)
        {
            var topic = _unitOfWork.Repository<Topic>().GetById(id);
            if (topic == null)
            {
                throw new Exception("Can not find Topic!!!");
            }
            string statusConvert = "";
            if (topic.Status == (int)TopicStatus.Status.New)
            {
                statusConvert = "Mới";
            }
            else if (topic.Status == (int)TopicStatus.Status.Waiting)
            {
                statusConvert = "Đang chờ kích hoạt";
            }
            else if (topic.Status == (int)TopicStatus.Status.Active)
            {
                statusConvert = "Đang diễn ra";
            }
            else if (topic.Status == (int)TopicStatus.Status.Disactive)
            {
                statusConvert = "Tạm dừng";
            }
            else if (topic.Status == (int)TopicStatus.Status.Closed)
            {
                statusConvert = "Đã đóng";
            }

            string topicInRoom = "Chủ đề này chưa được thiết lập phòng";
            if (topic.RoomId != null)
            {
                topicInRoom = "Chủ đề này đang ở phòng: " + topic.RoomId;
            }

            DateTime createDate = (DateTime)topic.CreateDate;
            DateTime startDate = (DateTime)topic.StartDate;
            TopicResponse topicResponse = new TopicResponse();
            topicResponse.Id = topic.Id;
            topicResponse.Name = topic.Name;
            topicResponse.Description = topic.Description;
            topicResponse.NameEng = topic.NameEng;
            topicResponse.DescriptionEng = topic.DescriptionEng;
            topicResponse.Image = topic.Image;
            topicResponse.CreateDate = createDate.Date.ToString("dd/MM/yyyy");
            topicResponse.StartDate = startDate.Date.ToString("dd/MM/yyyy");
            topicResponse.Rating = (float)topic.Rating;
            topicResponse.Status = statusConvert;
            topicResponse.isDelete = (bool)topic.IsDelete;
            topicResponse.RoomNo = topicInRoom;
            return topicResponse;
        }

        public List<TopicResponse> GetTopicHasNoRoom()
        {
            string statusConvert = "";
            var rs = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.IsDelete == false
                                                                    && t.Status != (int)TopicStatus.Status.Closed
                                                                    && t.RoomId == null).AsQueryable();

            List<TopicResponse> listTopicResponse = new List<TopicResponse>();
            foreach (var item in rs)
            {

                if (item.Status == (int)TopicStatus.Status.New)
                {
                    statusConvert = "Mới";
                }
                else if (item.Status == (int)TopicStatus.Status.Waiting)
                {
                    statusConvert = "Đang chờ kích hoạt";
                }
                else if (item.Status == (int)TopicStatus.Status.Active)
                {
                    statusConvert = "Đang diễn ra";
                }
                else if (item.Status == (int)TopicStatus.Status.Disactive)
                {
                    statusConvert = "Tạm dừng";
                }
                else if (item.Status == (int)TopicStatus.Status.Closed)
                {
                    statusConvert = "Đã đóng";
                }
                string topicInRoom = "Chủ đề này chưa được thiết lập phòng";
                
                DateTime createDate = (DateTime)item.CreateDate;
                DateTime startDate = (DateTime)item.StartDate;
                TopicResponse topicResponse = new TopicResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    NameEng = item.NameEng,
                    DescriptionEng = item.DescriptionEng,
                    Image = item.Image,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                    StartDate = startDate.Date.ToString("yyyy-MM-dd"),
                    Rating = (float)item.Rating,
                    Status = statusConvert,
                    isDelete = (bool)item.IsDelete,
                    RoomNo = topicInRoom
                };
                listTopicResponse.Add(topicResponse);    
            }
            return listTopicResponse.ToList();
        }

        public async Task<int> UpdateStatusFromWatingToActive(int id)
        {
            int rs = 0;
            //lấy topic ra by id
            Topic topic = _unitOfWork.Repository<Topic>().GetById(id);
            
            //nếu k có trả về msg
            if (topic == null)
            {
                throw new Exception("Cant Not Found This Topic!");
            }
           
            //check xem nó đã đc set room chưa        
            //int roomNo = (int)topic.RoomId;
                              
            //nếu chưa có phòng thì trả msg lỗi
            if (topic.RoomId == null)
            {
                throw new Exception("You must set room for this topic to active!!!");               
            }          
             
                try
                    {
                        topic.Status = (int) TopicStatus.Status.Active;
                        await _unitOfWork.CommitAsync();

                        rs = 1;
                        return rs;
                    }
                catch (Exception)
                {
                    throw new Exception("Update Error Because Some Problem From Server");    
                }  
       
        }

        //Implement from Interface ITopicService - cập nhập Topic
        public async Task<int> UpdateTopic(int id, string Name, string Description, string NameEng, string DescriptionEng, string Image, DateTime StartDate, string Status)
        {
            int statusToDb = 0;
            Topic topic = _unitOfWork.Repository<Topic>().GetById(id);
            if (topic == null)
            {
                throw new Exception("Cant Not Found This Topic!");
            }

            var listTopic = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.Id != id 
                                                                        && (t.Status != (int) TopicStatus.Status.Closed || t.IsDelete != true)
                                                                        && (t.Name.Equals(Name) || t.NameEng.Equals(NameEng))
                                                                        ).ToList();
            if (listTopic.Count() > 0)
            {
                throw new Exception("Tên tiếng Việt hoặc tên tiếng Anh của chủ đề đã bị trùng với chủ đề khác!!!");
            }

            if (Status == "Mới")
            {
                statusToDb = (int)TopicStatus.Status.New;
            }
            else if (Status == "Đang chờ kích hoạt")
            {              
                 statusToDb = (int)TopicStatus.Status.Waiting;                             
            } else if (Status == "Đang diễn ra")
            {
                statusToDb = (int)TopicStatus.Status.Active;
            }else if (Status == "Tạm dừng")
            {
                statusToDb = (int)TopicStatus.Status.Disactive;
            }else if (Status == "Đã đóng")
            {
                statusToDb = (int)TopicStatus.Status.Closed;
            }
           
            try
            {
                //nếu status đc update là closed, xóa obj ra khỏi đó, và xóa topic/Event In Room.
                if (statusToDb == (int)TopicStatus.Status.Closed)
                {
                    //chech xem có exhibit nào đang trong topic đó k để xóa ra 
                    var checkExhibitInTopic = _unitOfWork.Repository<ExhibitInTopic>().GetAll().Where(e => e.TopicId == id && e.Status == true).AsQueryable();
                    if (checkExhibitInTopic.Count() > 0)
                    {
                        //xóa exhibit ra khỏi topic
                       // _unitOfWork.Repository<ExhibitInTopic>().DeleteRange(checkExhibitInTopic);

                        //thay đổi status của exhibit thành Ready
                        foreach (var item in checkExhibitInTopic)
                        {
                            //update field status thành false
                            item.Status = false;

                            Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(item.ExhibitId);
                            //thay đổi status của exhibit thành ready
                            exhibit.Status = (int)ExhibitsStatus.Status.Ready;
                            _unitOfWork.Repository<Exhibit>().Update(exhibit, exhibit.Id);
                            //await _unitOfWork.CommitAsync();
                        }
                        //update 
                        _unitOfWork.Repository<ExhibitInTopic>().UpdateRange(checkExhibitInTopic);
                     
                    }
                    //check xem topic đó có đang ở room nào không để xóa ra
                    if (topic.RoomId != null)
                    {
                        Room room = _unitOfWork.Repository<Room>().GetAll().Where(r => r.Id == topic.RoomId).FirstOrDefault();
                        //set status của room thành 0
                        room.Status = (int) RoomStatus.Status.Ready;
                        _unitOfWork.Repository<Room>().Update(room, room.Id);

                        topic.RoomId = null;

                        
                    }

                                                         

                }

                topic.Name = Name;
                topic.Description = Description;
                topic.NameEng = NameEng;
                topic.DescriptionEng = DescriptionEng;
                topic.Image = Image;
                topic.StartDate = StartDate;
                topic.Status = statusToDb;
                
                await _unitOfWork.CommitAsync();
               
                return topic.Id;
            }
            catch (Exception)
            {
                throw new Exception("Update Error!!!");
            }
        }
    }
}
