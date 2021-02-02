using eTourGuide.Data.Entity;
using eTourGuide.Service.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTourGuide.Service.Services.InterfaceService
{
    public interface IFeedbackService
    {
        //Get list feedback(exhibit/event/topic)
        List<Feedback> GetFeedbacksExhibitcForUserById(int Id);

        List<Feedback> GetFeedbacksTopicForUserById(int Id);

        List<Feedback> GetFeedbacksEventForUserById(int Id);








        
    }
}
