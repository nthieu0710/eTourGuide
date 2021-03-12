using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace eTourGuide.Service.Exceptions
{
    public class FirebaseException : Exception
    {
        public HttpStatusCode Status { get; private set; }
        public FirebaseException(HttpStatusCode status, string msg) : base(msg)
        {
            Status = status;
        }
    }
}
