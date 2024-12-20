using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Exceptions
{
    public class CategoriaInvalidaException : ApplicationException
    {
        public CategoriaInvalidaException() 
            : base("Dados da Categoria inválidos.") { }
    }
}
