using System;
using System.Collections.Generic;
using System.Text;

namespace OCTO.Api.Adapter.Exceptions
{
    public class UnauthorizedException: ApiRepositoryException
    {
        public UnauthorizedException() : this("")
        {

        }

        public UnauthorizedException(string message)
        : base("Unauthorized \r\n" + message)
        {
        }

        public UnauthorizedException(string message, Exception inner)
        : base("Unauthorized \r\n" + message, inner)
        {

        }
        public UnauthorizedException(Exception inner)
        : base("Unauthorized", inner)
        {

        }
    }
}
