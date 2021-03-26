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

    public class ExhibitService : IExhibitService
    {
        private readonly IUnitOfWork _unitOfWork;
         
        public ExhibitService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        public async Task<int> AddExhibit(string Name, string Description, string NameEng, string DescriptionEng, string Image, TimeSpan duration)
        {
            int statusToDb = 0;

            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);

            Exhibit exhibit = new Exhibit
            {
                Name = Name,
                Description = Description,
                NameEng = NameEng,
                DescriptionEng = DescriptionEng,
                Image = Image,
                CreateDate = dtnew,
                Status = statusToDb,
                Duration = duration,
                IsDelete = false
            };
            try
            {
                await _unitOfWork.Repository<Exhibit>().InsertAsync(exhibit);
                await _unitOfWork.CommitAsync();

                return exhibit.Id;
            }
            catch (Exception)
            {
                throw new Exception("Insert Error!!!");
            }
            
        }

        public async Task<int> DeleteExhibit(int id)
        {
            Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(id);
            if (exhibit == null)
            {
                throw new Exception("Cant Not Found This Exhibit!");
            }
            if (exhibit.Status == 0)
            {
                try
                {
                    exhibit.IsDelete = true;
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception)
                {
                    throw new Exception("Can not delete exhibit!!!");
                }
            }
            else if (exhibit.Status == 1)
            {
                throw new Exception("Can not delete exhibit!!!");
            }
            return exhibit.Id;
        }

        public List<ExhibitResponse> GetAllExhibitForAdmin()
        {
            string statusConvert = "";
            var rs = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.IsDelete == false).AsQueryable();
            List<ExhibitResponse> listExhibitResponse = new List<ExhibitResponse>();
            foreach (var item in rs)
            {
                if (item.Status == 0)
                {
                    statusConvert = "Ready";
                }
                else if (item.Status == 1)
                {
                    statusConvert = "Added";
                }
               
                DateTime createDate = (DateTime)item.CreateDate;

                ExhibitResponse exhibitResponse = new ExhibitResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    NameEng = item.NameEng,
                    DescriptionEng = item.Description,
                    Image = item.Image,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                    Status = statusConvert,
                    Duration = (TimeSpan)item.Duration,
                    isDelete = (bool)item.IsDelete                 
                };
                listExhibitResponse.Add(exhibitResponse);                
            }
            return listExhibitResponse.ToList();
        }


        

        public List<ExhibitResponse> GetAllExhibitsForUser()
        {
            var rs = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Status == 1 && e.IsDelete == false).AsQueryable();

            List<ExhibitResponse> listExhibitResponse = new List<ExhibitResponse>();
            foreach (var item in rs)
            {
                if (item.ExhibitInEvents.Where(exInEvt => exInEvt.Status == true && exInEvt.Event.Status == 2).FirstOrDefault() != null
                    || item.ExhibitInTopics.Where(exInTopic => exInTopic.Status == true && exInTopic.Topic.Status == 2).FirstOrDefault() != null)
                {
                   
                    ExhibitResponse exhibitResponse = new ExhibitResponse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        NameEng = item.NameEng,
                        DescriptionEng = item.Description,
                        Image = item.Image,
                        //Rating = (float)item.Rating,
                        //TotalFeedback = count
                    };
                    listExhibitResponse.Add(exhibitResponse);

                }             
            }
            return listExhibitResponse.ToList();
        }

        public List<ExhibitResponse> GetAvailableExhibit()
        {
            var rs = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Status == 0 && e.IsDelete == false).AsQueryable();
            List<ExhibitResponse> listExhibitResponse = new List<ExhibitResponse>();
            foreach (var item in rs)
            {
                DateTime createDate = (DateTime)item.CreateDate;
                ExhibitResponse exhibitResponse = new ExhibitResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    NameEng = item.NameEng,
                    DescriptionEng = item.Description,
                    Image = item.Image,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                    Status = "Ready",
                    Duration = (TimeSpan)item.Duration
                };
                listExhibitResponse.Add(exhibitResponse);
            }
            return listExhibitResponse.ToList();
        }

       

        public List<ExhibitResponse> GetHightLightExhibit()
        {
            List<ExhibitResponse> listResponse = new List<ExhibitResponse>();

            //lấy ra list event
            var listExhibitInEvent = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(e => e.Status == true &&  e.Event.Status == 2).AsQueryable();
            //thêm vào list exhibit
            if (listExhibitInEvent != null)
            {
                foreach (var item in listExhibitInEvent)
                {
                    ExhibitResponse exhibit = new ExhibitResponse()
                    {
                        Id = item.Exhibit.Id,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        NameEng = item.Exhibit.NameEng,
                        DescriptionEng = item.Exhibit.DescriptionEng,
                        Image = item.Exhibit.Image,
                        Rating = (double)item.Event.Rating
                    };
                    listResponse.Add(exhibit);
                }
            }
            

            //lấy ra list topic
            var listExhibitInTopic = _unitOfWork.Repository<ExhibitInTopic>().GetAll().Where(t => t.Status == true && t.Topic.Status == 2).AsQueryable();
            //thêm vào list exhibit
            if (listExhibitInTopic != null)
            {
                foreach (var item in listExhibitInTopic)
                {
                    ExhibitResponse exhibit = new ExhibitResponse()
                    {
                        Id = item.Exhibit.Id,
                        Name = item.Exhibit.Name,
                        Description = item.Exhibit.Description,
                        NameEng = item.Exhibit.NameEng,
                        DescriptionEng = item.Exhibit.DescriptionEng,
                        Image = item.Exhibit.Image,
                        Rating = (double)item.Topic.Rating
                    };
                    listResponse.Add(exhibit);
                }
            }


            //sort list theo rating
            listResponse = listResponse.OrderByDescending(response => response.Rating).ToList();
            return listResponse.ToList();
        }

        

        public List<ExhibitResponse> GetNewExhibit()
        {
            int numberOfExhibitToDisplays = 20;
            var rs = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Status == 1 && e.IsDelete == false).AsQueryable();
            rs = rs.OrderByDescending(exhibit => exhibit.CreateDate);
            int count = 0;
            List<ExhibitResponse> listExhibitResponse = new List<ExhibitResponse>();
            foreach (var item in rs)
            {
                if (item.ExhibitInEvents.Where(exInEvt =>exInEvt.Status == true && exInEvt.Event.Status == 2).FirstOrDefault() != null
                      || item.ExhibitInTopics.Where(exInTopic =>exInTopic.Status == true && exInTopic.Topic.Status == 2).FirstOrDefault() != null)
                {
                    ExhibitResponse exhibitResponse = new ExhibitResponse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        NameEng = item.NameEng,
                        DescriptionEng = item.Description,
                        Image = item.Image,
                        //Rating = (float)item.Rating,
                    };
                    listExhibitResponse.Add(exhibitResponse);
                    count = count + 1;
                    if (count == numberOfExhibitToDisplays)
                    {
                        break;
                    }
                }                  
                            
            }
            return listExhibitResponse.ToList();
        }

        public async Task<int> UpdateExhibit(int id, string Name, string Description, string NameEng, string DescriptionEng, string Image, TimeSpan Duration)
        {
            Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(id);
            try
            {
                exhibit.Name = Name;
                exhibit.Description = Description;
                exhibit.NameEng = NameEng;
                exhibit.DescriptionEng = Description;
                exhibit.Image = Image;
                exhibit.Duration = Duration;

                await _unitOfWork.CommitAsync();

                return exhibit.Id;
            }
            catch (Exception)
            {
                throw new Exception("Update Error!!!");
            }
        }


        public List<ExhibitResponse> SearchExhibitForAdmin(string name)
        {
            List<ExhibitResponse> listExhibitResponse = new List<ExhibitResponse>();
            if(String.IsNullOrEmpty(name))
            {
                listExhibitResponse = GetAllExhibitForAdmin();
                return listExhibitResponse;
            }
            string statusConvert = "";
            var exhibit = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Name.Contains(name) && e.IsDelete == false).AsQueryable();
            
            foreach (var item in exhibit)
            {
                if (item.Status == 0)
                {
                    statusConvert = "Ready";
                }
                else if (item.Status == 1)
                {
                    statusConvert = "Added";
                }

                DateTime createDate = (DateTime)item.CreateDate;

                ExhibitResponse exhibitResponse = new ExhibitResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    NameEng = item.NameEng,
                    DescriptionEng = item.Description,
                    Image = item.Image,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd"),                 
                    Status = statusConvert,
                    Duration = (TimeSpan)item.Duration,
                    isDelete = (bool)item.IsDelete
                };
                listExhibitResponse.Add(exhibitResponse);
            }
            return listExhibitResponse.ToList();
        }

        public String GetTopicOrEventContainExhibit(int exhibitId)
        {
            string rs = "Không thuộc Chủ đề hay Sự kiện nào!";
            ExhibitInTopic topicContainExhibit = _unitOfWork.Repository<ExhibitInTopic>().GetAll().Where(t => t.ExhibitId == exhibitId && t.Status == true).FirstOrDefault();
            if (topicContainExhibit != null)
            {
                rs = "Đang thuộc về Chủ đề: " + topicContainExhibit.Topic.Name.ToString();
                return rs.ToString();
            }

            ExhibitInEvent eventContainExhibit = _unitOfWork.Repository<ExhibitInEvent>().GetAll().Where(e => e.ExhibitId == exhibitId && e.Status == true).FirstOrDefault();
            if (eventContainExhibit != null)
            {
                rs = "Đang thuộc về Chủ đề: " + eventContainExhibit.Event.Name.ToString();
                return rs.ToString();
            }
            return rs.ToString();
        }
    }
}
