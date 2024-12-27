using AutoMapper;
using ProductCatalogService.Application.Contracts;
using ProductCatalogService.Application.Dtos;
using ProductCatalogService.Domain.Contracts;
using ProductCatalogService.Application.Exceptions;
using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Domain.Pagination;
using System;
using ProductCatalogService.Domain.ValueObjects;

namespace ProductCatalogService.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IMapper _mapper;
        private readonly IProdutoRepository _repository; 

        public ProdutoService(IMapper mapper, IProdutoRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<DetailsProdutoDto> RegisterNewProdutoAsync(CreateProdutoDto createProdutoDto)
        {
            if (createProdutoDto == null) throw new ProdutoInvalidoException();

            var produto = _mapper.Map<Produto>(createProdutoDto);
            produto = await _repository.AddAsync(produto);

            DetailsProdutoDto produtoDto = _mapper.Map<DetailsProdutoDto>(produto);

            return produtoDto;
        }

        public async Task<DetailsProdutoDto> GetProdutoByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException();

            var produto = await _repository.GetByIdAsync(id);

            DetailsProdutoDto produtoDto = _mapper.Map<DetailsProdutoDto>(produto);

            return produtoDto;
        }

        public async Task<(MetaData, IEnumerable<DetailsProdutoDto>)> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco)
        {
            var produtos = (await _repository.GetAllAsync()).AsQueryable();

            if(produtosFiltroPreco.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroPreco.PrecoCriterio))
            {
                if(produtosFiltroPreco.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroPreco.Preco.Value).OrderBy(p => p.Preco);   
                }
                else if(produtosFiltroPreco.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltroPreco.Preco.Value).OrderBy(p => p.Preco);
                }
                else if(produtosFiltroPreco.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroPreco.Preco.Value).OrderBy(p => p.Preco);
                }
            }

            var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtosFiltroPreco.PageNumber, produtosFiltroPreco.PageSize);

            var metadata = new MetaData()
            {
                TotalCount = produtosFiltrados.TotalCount,
                PageSize = produtosFiltrados.PageSize,
                CurrentPage = produtosFiltrados.CurrentPage,
                TotalPages = produtosFiltrados.TotalPages,
                HasNext = produtosFiltrados.HasNext,
                HasPrevious = produtosFiltrados.HasPrevious
            };

            (MetaData, IEnumerable<DetailsProdutoDto>) tuplaRetorno = (metadata, _mapper.Map<IEnumerable<DetailsProdutoDto>>(produtosFiltrados));

            return tuplaRetorno;
        }

        public async Task<List<DetailsProdutoDto>> GetAllProdutosAtivosAsync(ProdutoParameters produtoParameters)
        {
            var produtos = await _repository.GetAllPagedAsync(produtoParameters);

            List<DetailsProdutoDto> produtosDto = _mapper.Map<List<DetailsProdutoDto>>(produtos);

            return produtosDto;
        }

        public async Task<DetailsProdutoDto> UpdateProdutoAsync(UpdateProdutoDto produtoDto, int id)
        {
            if (produtoDto == null) throw new ProdutoInvalidoException();
            if (id <= 0) throw new ArgumentException();

            var produto = _mapper.Map<Produto>(produtoDto);

            produto = await _repository.UpdateAsync(produto, id);

            DetailsProdutoDto detailsProduto = _mapper.Map<DetailsProdutoDto>(produto);

            return detailsProduto;

        }
        public async Task<ReadProdutoDto> DisableProdutoByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException();

            var produto = await _repository.RemoveAsync(id);

            ReadProdutoDto produtoDto = _mapper.Map<ReadProdutoDto>(produto);

            return produtoDto;
        }
    }
}
