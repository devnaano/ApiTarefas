using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTarefas.Models.Exceptions
{
    public class RegraDeNegocioException : Exception
    {
        public RegraDeNegocioException(string message) : base(message)
        {

        }
    }
}