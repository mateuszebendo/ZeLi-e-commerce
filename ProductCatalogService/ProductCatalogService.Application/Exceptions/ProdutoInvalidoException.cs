using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Exceptions
{
    public class ProdutoInvalidoException : ApplicationException
    {
        public ProdutoInvalidoException() : base("Dados do Produto inválidos.") { }
    }
}
