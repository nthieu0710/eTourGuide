using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IUserService
    {
        //SearchResponse SearchByName(string name);

        List<SearchResponseForUser> ConvertSearchList(string name);
    }
}
