using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace eTourGuide.Service.Exceptions
{
    public class CrudException : Exception
    {
        public HttpStatusCode Status { get; private set; }
        public CrudException(HttpStatusCode status, string msg) : base(msg)
        {
            Status = status;
        }
    }
}
