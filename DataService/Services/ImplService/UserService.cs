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

        /*public SearchResponse SearchByName(string name)
        {
            List<ExhibitResponse> listExhibitRs = SearchExhibitByName(name);
            List<TopicResponse> listtopicRs = SearchTopicByName(name);
            List<EventResponse> listEventRs = SearchEventByName(name);

            SearchResponse rs = new SearchResponse(listEventRs, listtopicRs, listExhibitRs);

            return rs;
        }*/

        public List<SearchResponseForUser> SearchByName(string name)
        {
            List<ExhibitResponse> listExhibitRs = SearchExhibitByName(name);
            List<TopicResponse> listtopicRs = SearchTopicByName(name);
            List<EventResponse> listEventRs = SearchEventByName(name);
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




         List<ExhibitResponse> SearchExhibitByName(string name)
        {
            var exhibit = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Name.Contains(name) && e.IsDelete == false && e.Status == 1).AsQueryable();
            List<ExhibitResponse> listExhibit = new List<ExhibitResponse>();
            if (exhibit != null)
            {
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
                    listExhibit.Add(obj);
                }
            }
            return listExhibit;
        }


         List<TopicResponse> SearchTopicByName(string name)
        {
            var topic = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.Name.Contains(name) && t.IsDelete == false && t.Status == 2).AsQueryable();
            List<TopicResponse> listTopic = new List<TopicResponse>();
            if (topic != null)
            {

                foreach (var item in topic)
                {
                    int count = 0;
                    var topicInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.TopicId == item.Id);
                   

                    DateTime dt = (DateTime)item.StartDate;
                    if (topicInFeedback != null)
                    {
                        count = topicInFeedback.Count();
                       
                    }

                    TopicResponse topicObj = new TopicResponse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Image = item.Image,
                        StartDate = dt.Date.ToString("dd/MM/yyyy"),
                        Rating = (float)item.Rating,
                        TotalFeedback = count

                    };                    
                    listTopic.Add(topicObj);                    
                }
            }
            return listTopic;
        }


         List<EventResponse> SearchEventByName(string name)
        {
            var evt = _unitOfWork.Repository<Event>().GetAll().Where(e => e.Name.Contains(name) && e.IsDelete == false && e.Status == 2).AsQueryable();
            List<EventResponse> listEvent = new List<EventResponse>();
            if (evt != null)
            {

                foreach (var item in evt)
                {
                    int count = 0;
                    var evtInFeedback = _unitOfWork.Repository<Feedback>().GetAll().Where(x => x.EventId == item.Id);
                   



                    if (evtInFeedback != null)
                    {
                        count = evtInFeedback.Count();
                       

                    }

                    DateTime startDate = (DateTime)item.StartDate;
                    DateTime endDate = (DateTime)item.EndDate;

                    EventResponse eventObj = new EventResponse()
                    {

                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Image = item.Image,
                        Rating = (double)item.Rating,
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
