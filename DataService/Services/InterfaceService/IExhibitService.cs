using eTourGuide.Data.Entity;
using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IExhibitService
    {
        //tạo mới 1 hiện vật
        Task<int> AddExhibit(string Name, string Description, string NameEng, string DescriptionEng, string Image, TimeSpan duration, string Username);

        //cập nhập 1 hiện vật
        Task<int> UpdateExhibit(int id, string Name, string Description, string NameEng, string DescriptionEng, string Image, TimeSpan Duration);

        //xóa 1 hiện vật
        Task<int> DeleteExhibit(int id);

        //get list những hiện vật chưa đc add vào topic/event nào
        List<ExhibitResponse> GetAvailableExhibit();

        //get all exhibit for admin
        List<ExhibitResponse> GetAllExhibitForAdmin();

        //search by name for admin
        List<ExhibitResponse> SearchExhibitForAdmin(string name);

        //Trả về Topic hoặc Event đang chứa hiện vật
        String GetTopicOrEventContainExhibit (int exhibitId);

        //------------------------------------------------------------------------------------------------------------------------------//
        //------------------------------------------------------------------------------------------------------------------------------//


        //get list những hiện vật vừa đc thêm gần đây
        List<ExhibitResponse> GetNewExhibit();

        //get list những hiện vật highlight rating > 4
        List<ExhibitResponse> GetHightLightExhibit();

        //get all exhibit đang có cho user
        List<ExhibitResponse> GetAllExhibitsForUser();

    }
}
