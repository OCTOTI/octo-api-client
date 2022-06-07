using System;
using System.Collections.Generic;
using System.Text;

namespace OCTO.Api.Adapter.Exceptions
{
    public class BadGatewayException : ApiRepositoryException
    {
        public BadGatewayException() : this("")
        {

        }

        public BadGatewayException(string message)
        : base(message)
        {
        }

        public BadGatewayException(string message, Exception inner)
        : base(message, inner)
        {

        }
        public BadGatewayException(Exception inner)
        : base("", inner)
        {

        }
    }
}
