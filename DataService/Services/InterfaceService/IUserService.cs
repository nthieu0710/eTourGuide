using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IUserService
    {
        //tìm kiếm event/topic/exhibit by name for user
        List<SearchResponseForUser> SearchByName(string language, string name);       
    }
}
