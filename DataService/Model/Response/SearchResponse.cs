using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class SearchResponse
    {
        

        public List<EventFeedbackResponse> _listEvent { get; set; }
        public List<TopicFeedbackResponse> _listTopic { get; set; }
        public List<ExhibitFeedbackResponse> _listExhibit { get; set; }

        public SearchResponse(List<EventFeedbackResponse> listEvent, List<TopicFeedbackResponse> listTopic, List<ExhibitFeedbackResponse> listExhibit)
        {
            _listEvent = listEvent;
            _listTopic = listTopic;
            _listExhibit = listExhibit;
        }

    }
}
