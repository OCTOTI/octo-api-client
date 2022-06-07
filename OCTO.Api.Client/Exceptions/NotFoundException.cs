using System;
using System.Collections.Generic;
using System.Text;

namespace OCTO.Api.Adapter.Exceptions
{
    public class NotFoundException : ApiRepositoryException
    {
        public NotFoundException() : this("")
        {

        }

        public NotFoundException(string message)
        : base("NotFound \r\n" + message)
        {

        }

        public NotFoundException(string message, Exception inner)
        : base("NotFound \r\n" + message, inner)
        {

        }
        public NotFoundException(Exception inner)
                : base("NotFound", inner)
        {

        }
    }
}
