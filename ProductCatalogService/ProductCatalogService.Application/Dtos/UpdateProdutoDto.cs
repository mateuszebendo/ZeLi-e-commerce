using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Dtos
{
    public class UpdateProdutoDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [MinLength(5, ErrorMessage = "A descrição deve ter no mínimo 5 caracteres")]
        public string Descricao { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Valor do preço inválido")]
        public double Preco { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Estoque inválido")]
        public double Estoque { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Categoria inválida")]
        public int CategoriaId { get; set; }

        [MaxLength(250, ErrorMessage = "O nome da imagem não pode exceder 250 caracteres")]
        public string ImagemURL { get; set; }
    }
}
