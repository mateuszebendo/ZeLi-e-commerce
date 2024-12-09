using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Domain.Entities
{
    public class Categoria
    {
        public int CategoriaID { get; set; }
        public String Nome { get; set; }
        public String Descricao { get; set; }
    }
}
