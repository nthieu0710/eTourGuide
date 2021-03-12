using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Model.Response
{
    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
        //public string StackTrade { get; set; }

        /*public ErrorResponse(Exception ex, string error)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
            StackTrade = error;
        }*/

        public ErrorResponse(Exception ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
            //StackTrade = error;
        }
    }
}
