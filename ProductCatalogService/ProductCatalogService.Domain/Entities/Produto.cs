using ProductCatalogService.Domain.Validation;

namespace ProductCatalogService.Domain.Entities
{
    public class Produto
    {
        private Produto() { } 

        public Produto(string nome, string descricao, double preco, double estoque, int categoriaId, string imagemURL)
        {
            ValidateDomain(nome, descricao, preco, estoque, categoriaId, imagemURL);
        }

        public int ProdutoID { get; set; }
        public String Nome { get; private set; }
        public String Descricao { get; private set; }
        public Double Preco { get; private set; }
        public Double Estoque { get; private set; }
        public int CategoriaId {  get; set; }
        public Categoria Categoria { get; set; }
        public String ImagemURL { get; private set; }
        public Boolean Ativo { get; set; } = true;

        public void Update(string nome, string descricao, double preco, double estoque, int categoriaId, string imagemUrl)
        {
            ValidateDomain(nome, descricao, preco, estoque, categoriaId, imagemUrl);
        }


        private void ValidateDomain(string nome, string descricao, double preco, double estoque, int categoriaId, string imagemUrl)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(nome), "O nome é obrigatório");
            DomainExceptionValidation.When(nome.Length < 3, "O nome deve ter no mínimo 3 caracteres");
            DomainExceptionValidation.When(string.IsNullOrEmpty(descricao), "A descrição é obrigatória");
            DomainExceptionValidation.When(descricao.Length < 5, "A descrição deve ter no mínimo 5 caracteres");
            DomainExceptionValidation.When(preco < 0, "Valor do preço inválido");
            DomainExceptionValidation.When(estoque < 0, "Estoque inválido");
            DomainExceptionValidation.When(categoriaId <= 0, "Categoria inválida");
            DomainExceptionValidation.When(imagemUrl?.Length > 250,
            "O nome da imagem não pode exceder 250 caracteres");

            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            Estoque = estoque;
            CategoriaId = categoriaId;
            ImagemURL = imagemUrl;
        }

    }
}
