using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class SuggestRouteResponse
    {
        public List<int> _listRoute { get; set; }
        public string _textRouteVi { get; set; }
        public string _textRouteEng { get; set; }

        public SuggestRouteResponse (List<int> listRoute, string textRouteVi, string textRouteEng)
        {
            _listRoute = listRoute;
            _textRouteVi = textRouteVi;
            _textRouteEng = textRouteEng;
        }
    }
}
