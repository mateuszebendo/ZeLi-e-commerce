using ProductCatalogService.Application.Dtos;
using ProductCatalogService.Domain.Pagination;
using ProductCatalogService.Domain.ValueObjects;

namespace ProductCatalogService.Application.Contracts
{
    public interface IProdutoService
    {
        public Task<DetailsProdutoDto> RegisterNewProdutoAsync(CreateProdutoDto createProdutoDto);
        public Task<DetailsProdutoDto> GetProdutoByIdAsync(int id);
        public Task<(MetaData, IEnumerable<DetailsProdutoDto>)> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco);
        public Task<List<DetailsProdutoDto>> GetAllProdutosAtivosAsync(ProdutoParameters produtoParameters);
        public Task<DetailsProdutoDto> UpdateProdutoAsync(UpdateProdutoDto produtoDto, int id);
        public Task<ReadProdutoDto> DisableProdutoByIdAsync(int id);
    }
}
