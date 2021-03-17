using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class SearchResponse
    {
        

        public List<EventResponse> _listEvent { get; set; }
        public List<TopicResponse> _listTopic { get; set; }
        public List<ExhibitResponse> _listExhibit { get; set; }

        public SearchResponse(List<EventResponse> listEvent, List<TopicResponse> listTopic, List<ExhibitResponse> listExhibit)
        {
            _listEvent = listEvent;
            _listTopic = listTopic;
            _listExhibit = listExhibit;
        }

    }
}
