using AutoMapper;
using ProductCatalogService.Application.Dtos;
using ProductCatalogService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Profiles
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            CreateMap<CreateCategoriaDto, Categoria>();
            CreateMap<UpdateCategoriaDto, Categoria>();
            CreateMap<Categoria, ReadCategoriaDto>();
            CreateMap<Categoria, DetailsCategoriaDto>();
        }
    }
}
