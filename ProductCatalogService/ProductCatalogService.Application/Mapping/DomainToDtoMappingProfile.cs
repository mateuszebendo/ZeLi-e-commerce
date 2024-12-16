using AutoMapper;
using ProductCatalogService.Application.Dtos;
using ProductCatalogService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Mapping
{
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            CreateMap<CreateProdutoDto, Produto>()
                .ReverseMap();
            CreateMap<UpdateProdutoDto, Produto>()
                .ReverseMap();
            CreateMap<Produto, ReadProdutoDto>()
                .ReverseMap();
            CreateMap<Produto, DetailsProdutoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProdutoID))
                .ReverseMap();

            CreateMap<CreateCategoriaDto, Categoria>();
            CreateMap<UpdateCategoriaDto, Categoria>();
            CreateMap<Categoria, ReadCategoriaDto>();
            CreateMap<Categoria, DetailsCategoriaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoriaID));
        }
    }
}
