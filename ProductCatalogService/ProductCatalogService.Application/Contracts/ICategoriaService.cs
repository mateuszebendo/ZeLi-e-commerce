using ProductCatalogService.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Contracts
{
    public interface ICategoriaService
    {
        public Task<DetailsCategoriaDto> RegisterNewCategoriaAsync(CreateCategoriaDto createCategoriaDto);
        public Task<DetailsCategoriaDto> GetCategoriaByIdAsync(int id);
        public Task<List<DetailsCategoriaDto>> GetAllCategoriasAtivasAsync();
        public Task<DetailsCategoriaDto> UpdateCategoriaAsync(UpdateCategoriaDto categoriaDto, int id);
        public Task<ReadCategoriaDto> DisableCategoriaByIdAsync(int id);
    }
}
