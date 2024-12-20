using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Dtos
{
    public class ReadCategoriaDto
    {
        [Required(ErrorMessage = "O nome da categoria é obrigatório", AllowEmptyStrings = false)]
        [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres")]
        public String Nome { get; set; }
        [Required(ErrorMessage = "descrição da categoria é obrigatória", AllowEmptyStrings = false)]
        [MinLength(5, ErrorMessage = "A descrição deve ter no mínimo 5 caracteres")]
        public String Descricao { get; set; }
    }
}
