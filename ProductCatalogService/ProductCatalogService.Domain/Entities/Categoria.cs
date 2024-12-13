using ProductCatalogService.Domain.Validation;

namespace ProductCatalogService.Domain.Entities
{
    public class Categoria
    {
        private Categoria() { }

        public Categoria(string nome, string descricao)
        {
            ValidateDomain(nome, descricao);
        }

        public int CategoriaID { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; set; } = true;

        public void Update(string nome, string descricao)
        {
            ValidateDomain(nome, descricao);
        }

        private void ValidateDomain(string nome, string descricao)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(nome), "O nome é obrigatório");
            DomainExceptionValidation.When(nome.Length < 3, "O nome deve ter no mínimo 3 caracteres");
            DomainExceptionValidation.When(string.IsNullOrEmpty(descricao), "A descrição é obrigatória");
            DomainExceptionValidation.When(descricao.Length < 5, "A descrição deve ter no mínimo 5 caracteres");

            Nome = nome;
            Descricao = descricao;
        }
    }
}
