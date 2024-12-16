using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Dtos
{
    public class ReadProdutoDto
    {
        public String Nome { get; set; }
        public String Descricao { get; set; }
        public Double Preco { get; set; }
        public Double Estoque { get; set; }
        public ReadCategoriaDto Categoria { get; set; }
        public String ImagemURL { get; set; }
    }
}
