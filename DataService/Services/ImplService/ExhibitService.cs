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

        public async Task<Exhibit> AddExhibit(string Name, string Description, string Image, float Rating, int Status)
        {
            Exhibit exhibit = new Exhibit
            {
                Name = Name,
                Description = Description,
                Image = Image,
                CreateDate = DateTime.Now,
                Rating = Rating,
                Status = Status
            };
            try
            {
                await _unitOfWork.Repository<Exhibit>().InsertAsync(exhibit);
                await _unitOfWork.CommitAsync();
               
                return exhibit;
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Insert Error!!!");
            }
            }

        public List<ExhibitResponseForUser> GetAllExhibitsForUser()
        {
            var rs = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Status == 1).AsQueryable();
            List<ExhibitResponseForUser> listExhibitResponse = new List<ExhibitResponseForUser>();
            foreach (var item in rs)
            {               
                    ExhibitResponseForUser exhibitResponse = new ExhibitResponseForUser()
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

        public List<ExhibitFeedbackResponse> GetHightLightExhibit()
        {
            var exhibit = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Status == 1).AsQueryable();
            List<ExhibitFeedbackResponse> listExhibit = new List<ExhibitFeedbackResponse>();
            foreach (var item in exhibit)
            {
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
                ExhibitFeedbackResponse obj = new ExhibitFeedbackResponse()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Image = item.Image,
                    Rating = ratingAVG,
                    TotalFeedback = count 
                };
                if (obj.Rating >= 4)
                {
                    listExhibit.Add(obj);
                }
            }
            return listExhibit;
        }

        public List<ExhibitResponseForUser> GetNewExhibit()
        {
            var rs = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Status == 1).AsQueryable();
            rs = rs.OrderByDescending(exhibit => exhibit.CreateDate);
            int count = 0;
            List<ExhibitResponseForUser> listExhibitResponse = new List<ExhibitResponseForUser>();
            foreach (var item in rs)
            {
             
                
                    ExhibitResponseForUser exhibitResponse = new ExhibitResponseForUser()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Image = item.Image,
                        Rating = (float)item.Rating,
                    };
                    listExhibitResponse.Add(exhibitResponse);
                    count = count + 1;
                    if (count == 10)
                    {
                        break;
                    }
                            
            }
  
            return listExhibitResponse.ToList();
        }
    }
}
