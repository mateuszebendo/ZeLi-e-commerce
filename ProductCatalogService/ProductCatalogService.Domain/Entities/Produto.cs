using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Domain.Entities
{
    public class Produto
    {
        public int ProdutoID { get; set; }
        public String Nome { get; set; }
        public String Descricao { get; set; }
        public Double Preco { get; set; }
        public Double Estoque { get; set; }
        public Categoria Categoria { get; set; }
        public String ImagemURL { get; set; }
    }
}
