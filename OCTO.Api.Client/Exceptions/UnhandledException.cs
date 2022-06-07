using System;
using System.Collections.Generic;
using System.Text;

namespace OCTO.Api.Adapter.Exceptions
{
    public class UnhandledException : ApiRepositoryException
    {


        public UnhandledException(string message)
        : base(message)
        {
        }

        public UnhandledException(string message, Exception inner)
        : base(message, inner)
        {

        }
        
    }
}
