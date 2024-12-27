using AutoMapper;
using ProductCatalogService.Application.Contracts;
using ProductCatalogService.Application.Dtos;
using ProductCatalogService.Domain.Contracts;
using ProductCatalogService.Domain.Entities;
using ProductCatalogService.Application.Exceptions;
using ProductCatalogService.Domain.Pagination;
using ProductCatalogService.Domain.ValueObjects;

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

        public async Task<(MetaData, IEnumerable<DetailsCategoriaDto>)> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasFiltro)
        {
            var categorias = (await _repository.GetAllAsync()).AsQueryable(); 

            if(!string.IsNullOrEmpty(categoriasFiltro.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasFiltro.Nome));
            }

            var categoriasFiltradas = PagedList<Categoria>
                .ToPagedList(categorias, categoriasFiltro.PageNumber, categoriasFiltro.PageSize);

            var metadata = new MetaData()
            {
                TotalCount = categoriasFiltradas.TotalCount,
                PageSize = categoriasFiltradas.PageSize,
                CurrentPage = categoriasFiltradas.CurrentPage,
                TotalPages = categoriasFiltradas.TotalPages,
                HasNext = categoriasFiltradas.HasNext,
                HasPrevious = categoriasFiltradas.HasPrevious
            };

            var tuplaRetornoService = (metadata, _mapper.Map<IEnumerable<DetailsCategoriaDto>>(categoriasFiltradas));

            return tuplaRetornoService;
        }

        public async Task<(MetaData, IEnumerable<DetailsCategoriaDto>)> GetAllCategoriasAtivasPagedAsync(CategoriaParameters categoriaParameters)
        {
            var categorias = await _repository.GetAllPagedAsync(categoriaParameters);

            var metadata = new MetaData()
            {
                TotalCount = categorias.TotalCount,
                PageSize = categorias.PageSize,
                CurrentPage = categorias.CurrentPage,
                TotalPages = categorias.TotalPages,
                HasNext = categorias.HasNext,
                HasPrevious = categorias.HasPrevious
            };

            var detailsCategoriaDtos = _mapper.Map<IEnumerable<DetailsCategoriaDto>>(categorias);

            var tuplaRetornoService = (metadata, detailsCategoriaDtos);

            return tuplaRetornoService;
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
