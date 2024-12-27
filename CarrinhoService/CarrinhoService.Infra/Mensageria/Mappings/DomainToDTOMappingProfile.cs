using AutoMapper;
using CarrinhoService.Application.DTOs;
using CarrinhoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrinhoService.Application.Mappings;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Produto, ProdutoAtualizadoEvent>().ReverseMap();
        CreateMap<Produto, ProdutoInfoDto>().ReverseMap();
    }
}
