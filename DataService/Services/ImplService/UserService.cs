using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eTourGuide.Service.Services.ImplService
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        public SearchResponse SearchByName(string name)
        {
            List<ExhibitFeedbackResponse> listExhibitRs = SearchExhibitByName(name);
            List<TopicFeedbackResponse> listtopicRs = SearchTopicByName(name);
            List<EventFeedbackResponse> listEventRs = SearchEventByName(name);

            SearchResponse rs = new SearchResponse(listEventRs, listtopicRs, listExhibitRs);

            return rs;
        }

        public List<SearchResponseForUser> ConvertSearchList(string name)
        {
            List<ExhibitFeedbackResponse> listExhibitRs = SearchExhibitByName(name);
            List<TopicFeedbackResponse> listtopicRs = SearchTopicByName(name);
            List<EventFeedbackResponse> listEventRs = SearchEventByName(name);
            SearchResponse searchResponse = new SearchResponse(listEventRs, listtopicRs, listExhibitRs);
            List<SearchResponseForUser> rs = new List<SearchResponseForUser>();

            foreach(var item in searchResponse._listExhibit)
            {
                SearchResponseForUser exhibitConvert = new SearchResponseForUser
                {
                      Id = item.Id,
                      Name = item.Name,
                      Description = item.Description,
                      Image = item.Image,
                      Rating = item.Rating,
                      TotalFeedback = item.TotalFeedback,
                      Type = "Exhibit"
                };
                rs.Add(exhibitConvert);
            }

            foreach (var item in searchResponse._listEvent)
            {
                SearchResponseForUser eventConvert = new SearchResponseForUser
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Image = item.Image,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Rating = item.Rating,
                    TotalFeedback = item.TotalFeedback,
                    Type = "Event"
                };
                rs.Add(eventConvert);
            }

            foreach (var item in searchResponse._listTopic)
            {
                SearchResponseForUser topicConvert = new SearchResponseForUser
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Image = item.Image,
                    StartDate = item.StartDate,
                    Rating = item.Rating,
                    TotalFeedback = item.TotalFeedback,
                    Type = "Topic"
                };
                rs.Add(topicConvert);
            }

            return rs;

        }




         List<ExhibitFeedbackResponse> SearchExhibitByName(string name)
        {
            var exhibit = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Name.Contains(name) && e.IsDelete == false && e.Status == 1).AsQueryable();
            List<ExhibitFeedbackResponse> listExhibit = new List<ExhibitFeedbackResponse>();
            if (exhibit != null)
            {
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
                    listExhibit.Add(obj);
                }
            }
            return listExhibit;
        }


         List<TopicFeedbackResponse> SearchTopicByName(string name)
        {
            var topic = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.Name.Contains(name) && t.IsDelete == false && t.Status == 2).AsQueryable();
            List<TopicFeedbackResponse> listTopic = new List<TopicFeedbackResponse>();
            if (topic != null)
            {

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
                    listTopic.Add(topicObj);                    
                }
            }
            return listTopic;
        }


         List<EventFeedbackResponse> SearchEventByName(string name)
        {
            var evt = _unitOfWork.Repository<Event>().GetAll().Where(e => e.Name.Contains(name) && e.IsDelete == false && e.Status == 2).AsQueryable();
            List<EventFeedbackResponse> listEvent = new List<EventFeedbackResponse>();
            if (evt != null)
            {

                foreach (var item in evt)
                {
                    int count = 0;
                    var evtInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.EventId == item.Id);
                    double ratingAVG = 0;
                    double sumRating = 0;



                    if (evtInFeedback != null)
                    {
                        count = evtInFeedback.Count();
                        foreach (var item2 in evtInFeedback)
                        {
                            sumRating = (double)(sumRating + item2.Rating);
                        }
                        if (count != 0)
                        {
                            ratingAVG = sumRating / count;
                        }

                    }

                    DateTime startDate = (DateTime)item.StartDate;
                    DateTime endDate = (DateTime)item.EndDate;

                    EventFeedbackResponse eventObj = new EventFeedbackResponse()
                    {

                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Image = item.Image,
                        Rating = Math.Round((float)ratingAVG, 2),
                        StartDate = startDate.Date.ToString("dd/MM/yyyy"),
                        EndDate = endDate.Date.ToString("dd/MM/yyyy"),
                        TotalFeedback = count

                    };
                   
                        listEvent.Add(eventObj);
                    
                }

            }
            return listEvent;
        }

    }
}
