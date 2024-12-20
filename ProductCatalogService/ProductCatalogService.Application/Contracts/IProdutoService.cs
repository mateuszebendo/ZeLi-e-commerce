using ProductCatalogService.Application.Dtos;

namespace ProductCatalogService.Application.Contracts
{
    public interface IProdutoService
    {
        public Task<DetailsProdutoDto> RegisterNewProdutoAsync(CreateProdutoDto createProdutoDto);
        public Task<DetailsProdutoDto> GetProdutoByIdAsync(int id);
        public Task<List<DetailsProdutoDto>> GetAllProdutosAtivosAsync();
        public Task<DetailsProdutoDto> UpdateProdutoAsync(UpdateProdutoDto produtoDto, int id);
        public Task<ReadProdutoDto> DisableProdutoByIdAsync(int id);
    }
}
