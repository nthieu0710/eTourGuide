using eTourGuide.Data.Entity;
using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Servcies.InterfaceService
{
    public interface IAccountService
    {
       //Đăng nhập lần đầu 
        Task<string> AuthenticateAsync(string username, string password);

        //Xác thực đăng nhập
        VerifyResponse VerifyJwtLogin(string jwt);
    }
}
