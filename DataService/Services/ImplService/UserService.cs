using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;
using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using eTourGuide.Service.Helpers;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Services.InterfaceService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace eTourGuide.Service.Services.ImplService
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomService _roomService;

        public UserService(IUnitOfWork unitOfWork, IRoomService roomService)
        {
            _unitOfWork = unitOfWork;
            _roomService = roomService;
        }

        
        //tìm kím topic/event/exhibits by name
        public List<SearchResponseForUser> SearchByName(string language, string name)
        {
            List<ExhibitResponse> listExhibitRs = new List<ExhibitResponse>();
            List<TopicResponse> listtopicRs = new List<TopicResponse>();
            List<EventResponse> listEventRs = new List<EventResponse>();

            string english = "en";
            string vietnamese = "vi";

            if (language == english) 
            {
                 listExhibitRs = SearchExhibitByName(english, name);
                 listtopicRs = SearchTopicByName(english, name);
                 listEventRs = SearchEventByName(english, name);
            }
            else if (language == vietnamese)
            {
                listExhibitRs = SearchExhibitByName(vietnamese, name);
                listtopicRs = SearchTopicByName(vietnamese, name);
                listEventRs = SearchEventByName(vietnamese, name);
            }

            
            SearchResponse searchResponse = new SearchResponse(listEventRs, listtopicRs, listExhibitRs);
            List<SearchResponseForUser> rs = new List<SearchResponseForUser>();

            foreach(var item in searchResponse._listExhibit)
            {
                SearchResponseForUser exhibitConvert = new SearchResponseForUser
                {
                      Id = item.Id,
                      Name = item.Name,
                      Description = item.Description,
                      NameEng = item.NameEng,
                      DescriptionEng = item.DescriptionEng,
                      Image = item.Image,
                      //Rating = item.Rating,
                      //TotalFeedback = item.TotalFeedback,
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
                    NameEng = item.NameEng,
                    DescriptionEng = item.DescriptionEng,
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
                    NameEng = item.NameEng,
                    DescriptionEng = item.DescriptionEng,
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

        //search exhibits by name
         List<ExhibitResponse> SearchExhibitByName(string language, string name)
        {
            var exhibit = new List<Exhibit>();
            string vietnamese = "vi";
            string english = "en";

            if (language == vietnamese)
            {
                exhibit = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.Name.Contains(name) && e.IsDelete == false && e.Status == 1).AsQueryable().ToList();
            }
            else if (language == english)
            {
                exhibit = _unitOfWork.Repository<Exhibit>().GetAll().Where(e => e.NameEng.Contains(name) && e.IsDelete == false && e.Status == 1).AsQueryable().ToList();
            }

            List<ExhibitResponse> listExhibit = new List<ExhibitResponse>();
            if (exhibit != null)
            {
                foreach (var item in exhibit)
                {

                    if (item.ExhibitInEvents.Where(exInEvt => exInEvt.Status == true 
                                                              && exInEvt.Event.Status == (int) EventStatus.Status.Active 
                                                              && DateTime.Now >= exInEvt.Event.StartDate
                                                              && DateTime.Now <= exInEvt.Event.EndDate).FirstOrDefault() != null 
                                                              ||
                         item.ExhibitInTopics.Where(exInTopic => exInTopic.Status == true  
                                                                 && exInTopic.Topic.Status == (int) TopicStatus.Status.Active
                                                                 && DateTime.Now >= exInTopic.Topic.StartDate).FirstOrDefault() != null)
                    {

                        ExhibitResponse obj = new ExhibitResponse()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description,
                            NameEng = item.NameEng,
                            DescriptionEng = item.DescriptionEng,
                            Image = item.Image,
                            //Rating = (double)item.Rating,
                            //TotalFeedback = count
                        };
                        listExhibit.Add(obj);

                    }

                   
                }
            }
            return listExhibit;
        }


         List<TopicResponse> SearchTopicByName(string language, string name)
        {
            var topic = new List<Topic>();
            string vietnamese = "vi";
            string english = "en";

            if (language == vietnamese)
            {
                topic = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.Name.Contains(name) 
                                                                            && t.IsDelete == false 
                                                                            && t.Status == (int) TopicStatus.Status.Active
                                                                            && DateTime.Now >= t.StartDate).AsQueryable().ToList();
            }
            else if (language == english)
            {
                topic = _unitOfWork.Repository<Topic>().GetAll().Where(t => t.NameEng.Contains(name) 
                                                                            && t.IsDelete == false 
                                                                            && t.Status == (int) TopicStatus.Status.Active
                                                                            && DateTime.Now >= t.StartDate).AsQueryable().ToList();
            }


            
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
                        NameEng = item.NameEng,
                        DescriptionEng = item.DescriptionEng,
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


         List<EventResponse> SearchEventByName(string language, string name)
        {
            var evt = new List<Event>();
            string vietnamese = "vi";
            string english = "en";

            if (language == vietnamese)
            {
                evt = _unitOfWork.Repository<Event>().GetAll().Where(e => e.Name.Contains(name) 
                                                                          && e.IsDelete == false 
                                                                          && e.Status == (int) EventStatus.Status.Active
                                                                          && DateTime.Now >= e.StartDate
                                                                          && DateTime.Now <= e.EndDate).AsQueryable().ToList();
            } else if (language == english)
            {
                evt = _unitOfWork.Repository<Event>().GetAll().Where(e => e.NameEng.Contains(name) 
                                                                          && e.IsDelete == false 
                                                                          && e.Status == (int) EventStatus.Status.Active
                                                                          && DateTime.Now >= e.StartDate
                                                                          && DateTime.Now <= e.EndDate).AsQueryable().ToList();
            }

             
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
                        NameEng = item.NameEng,
                        DescriptionEng = item.DescriptionEng,
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
