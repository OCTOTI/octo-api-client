using System;
using System.Collections.Generic;
using System.Text;

namespace OCTO.Api.Adapter.Exceptions
{
    public abstract class ApiRepositoryException : Exception
    {
        public ApiRepositoryException(string message)
        : base("A requisição retornou o Erro: " + message)
        {
        }

        public ApiRepositoryException(string message, Exception inner)
        : base("A requisição retornou o Erro: " + message, inner)
        {
        }
    }
}
