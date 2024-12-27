using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Domain.Pagination
{
    public class ProdutosFiltroPreco : PaginationParameters
    {
        public double? Preco { get; set; }
        public string? PrecoCriterio { get; set; }
    }
}
