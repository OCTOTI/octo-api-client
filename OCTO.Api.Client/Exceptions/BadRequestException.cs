using System;
using System.Collections.Generic;
using System.Text;

namespace OCTO.Api.Adapter.Exceptions
{
    public class BadRequestException : ApiRepositoryException
    {
        public BadRequestException() : this("")
        {

        }

        public BadRequestException(string message)
        : base(message)
        {
        }

        public BadRequestException(string message, Exception inner)
        : base(message, inner)
        {

        }
        public BadRequestException(Exception inner)
        : base("", inner)
        {

        }

    }

}
