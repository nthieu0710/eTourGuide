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

        

        public async Task<int> AddExhibit(string Name, string Description, string Image, TimeSpan duration)
        {
            int statusToDb = 0;
            DateTime dt = Convert.ToDateTime(DateTime.Now);
            string s2 = dt.ToString("yyyy-MM-dd");
            DateTime dtnew = Convert.ToDateTime(s2);
            Exhibit exhibit = new Exhibit
            {
                Name = Name,
                Description = Description,
                Image = Image,
                CreateDate = dtnew,
                Rating = 0,
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
            Exhibit exhibit= _unitOfWork.Repository<Exhibit>().GetById(id);
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
                    Image = item.Image,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                    Rating = (float)item.Rating,
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
                ExhibitResponse exhibitResponse = new ExhibitResponse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Image = item.Image,
                        Rating = (float)item.Rating,
                    };
                    listExhibitResponse.Add(exhibitResponse);
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
                    Image = item.Image,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                    Rating = (float)item.Rating,
                    Status = "Ready",
                    Duration = (TimeSpan)item.Duration
                };
                listExhibitResponse.Add(exhibitResponse);
            }
            return listExhibitResponse.ToList();
        }

        public List<ExhibitResponse> GetHightLightExhibit()
        {
            int highlightRate = 4;
            var exhibit = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Status == 1 && e.IsDelete == false).AsQueryable();
            List<ExhibitResponse> listExhibit = new List<ExhibitResponse>();
            foreach (var item in exhibit)
            {
                int count = 0;
                var exhibitInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.ExhibittId == item.Id);
              
                if (exhibitInFeedback != null)
                {
                    count = exhibitInFeedback.Count();
                 

                }
                ExhibitResponse obj = new ExhibitResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Image = item.Image,
                    Rating = (double)item.Rating,
                    TotalFeedback = count 
                };
                if (obj.Rating >= highlightRate)
                {
                    listExhibit.Add(obj);
                }
            }
            return listExhibit.ToList();
        }

        public List<ExhibitResponse> GetNewExhibit()
        {
            int numberOfExhibitToDisplays = 10;
            var rs = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Status == 1 && e.IsDelete == false).AsQueryable();
            rs = rs.OrderByDescending(exhibit => exhibit.CreateDate);
            int count = 0;
            List<ExhibitResponse> listExhibitResponse = new List<ExhibitResponse>();
            foreach (var item in rs)
            {


                    ExhibitResponse exhibitResponse = new ExhibitResponse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Image = item.Image,
                        Rating = (float)item.Rating,
                    };
                    listExhibitResponse.Add(exhibitResponse);
                    count = count + 1;
                    if (count == numberOfExhibitToDisplays)
                    {
                        break;
                    }
                            
            }
  
            return listExhibitResponse.ToList();
        }

        public async Task<int> UpdateExhibit(int id, string Name, string Description, string Image, TimeSpan Duration)
        {
            Exhibit exhibit = _unitOfWork.Repository<Exhibit>().GetById(id);
            try
            {
                exhibit.Name = Name;
                exhibit.Description = Description;
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
                    Image = item.Image,
                    CreateDate = createDate.Date.ToString("yyyy-MM-dd"),
                    Rating = (float)item.Rating,
                    Status = statusConvert,
                    Duration = (TimeSpan)item.Duration,
                    isDelete = (bool)item.IsDelete
                };
                listExhibitResponse.Add(exhibitResponse);
            }
            return listExhibitResponse.ToList();
        }
    }
}
