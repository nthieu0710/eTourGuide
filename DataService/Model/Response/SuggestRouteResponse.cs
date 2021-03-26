using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class SuggestRouteResponse
    {
        public List<int> _listRoute { get; set; }
        public string _textRoute { get; set; }

        public SuggestRouteResponse (List<int> listRoute, string textRoute)
        {
            _listRoute = listRoute;
            _textRoute = textRoute;
        }
    }
}
