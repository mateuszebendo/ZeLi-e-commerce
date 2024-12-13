using AutoMapper;
using ProductCatalogService.Application.Contracts;
using ProductCatalogService.Application.Dtos;
using ProductCatalogService.Domain.Contracts;

namespace ProductCatalogService.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;
        private readonly IMapper _mapper;

        public CategoriaService(ICategoriaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<DetailsCategoriaDto> RegisterNewCategoriaAsync(CreateCategoriaDto createCategoriaDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<DetailsCategoriaDto>> GetAllCategoriasAtivasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DetailsCategoriaDto> GetCategoriaByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DetailsCategoriaDto> UpdateCategoriaAsync(UpdateCategoriaDto categoriaDto, int id)
        {
            throw new NotImplementedException();
        }
        public Task<ReadCategoriaDto> DisableCategoriaByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
