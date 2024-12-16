using AutoMapper;
using ProductCatalogService.Application.Contracts;
using ProductCatalogService.Application.Dtos;
using ProductCatalogService.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<ReadProdutoDto> DisableProdutoByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DetailsProdutoDto>> GetAllProdutosAtivosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DetailsProdutoDto> GetProdutoByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DetailsProdutoDto> RegisterNewProdutoAsync(CreateProdutoDto createProdutoDto)
        {
            throw new NotImplementedException();
        }

        public Task<DetailsProdutoDto> UpdateProdutoAsync(UpdateProdutoDto produtoDto, int id)
        {
            throw new NotImplementedException();
        }
    }
}
