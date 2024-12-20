using AutoMapper;
using ProductCatalogService.Application.Contracts;
using ProductCatalogService.Application.Dtos;
using ProductCatalogService.Domain.Contracts;
using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Application.Exceptions;

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

        public async Task<DetailsCategoriaDto?> RegisterNewCategoriaAsync(CreateCategoriaDto createCategoriaDto)
        {
            if (createCategoriaDto == null) throw new CategoriaInvalidaException();

            var categoria = _mapper.Map<Categoria>(createCategoriaDto);
            categoria = await _repository.AddAsync(categoria);

            var detailsCategoriaDto = _mapper.Map<DetailsCategoriaDto>(categoria);

            return detailsCategoriaDto;
        }

        public async Task<List<DetailsCategoriaDto?>> GetAllCategoriasAtivasAsync()
        {
            var categorias = await _repository.GetAllAsync();
            List<DetailsCategoriaDto> detailsCategoriaDtos = new List<DetailsCategoriaDto>();
            foreach(var categoria in categorias)
            {
                detailsCategoriaDtos.Add(_mapper.Map<DetailsCategoriaDto>(categoria));
            }

            return detailsCategoriaDtos;
        }

        public async Task<DetailsCategoriaDto?> GetCategoriaByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException();

            var categoria = await _repository.GetByIdAsync(id);
            var detailsCategoriaDto = _mapper.Map<DetailsCategoriaDto>(categoria);

            return detailsCategoriaDto;
        }

        public async Task<DetailsCategoriaDto?> UpdateCategoriaAsync(UpdateCategoriaDto categoriaDto, int id)
        {
            if (categoriaDto == null) throw new CategoriaInvalidaException();

            if(id <= 0) throw new ArgumentException();

            var categoria = _mapper.Map<Categoria>(categoriaDto);
            categoria = await _repository.UpdateAsync(categoria, id);

            DetailsCategoriaDto detailsCategoriaDto = _mapper.Map<DetailsCategoriaDto>(categoria);
            detailsCategoriaDto.Id = id;

            return detailsCategoriaDto;
        }
        public async Task<ReadCategoriaDto?> DisableCategoriaByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException();

            var categoria = await _repository.RemoveAsync(id);

            ReadCategoriaDto categoriaDto = _mapper.Map<ReadCategoriaDto>(categoria);
            return categoriaDto;
        }
    }
}
