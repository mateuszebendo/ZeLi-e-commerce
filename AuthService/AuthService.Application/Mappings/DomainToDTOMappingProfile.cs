using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Mappings;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Usuario, UsuarioDto>()
            .ForMember(dest => dest.Cargo, opt => opt.MapFrom(src => src.Cargo.ToString()))
            .ReverseMap();
        CreateMap<Usuario, UsuarioCreateDto>().ReverseMap();
        CreateMap<UsuarioDto, UsuarioLoginResponseDto>().ReverseMap();
    }
}
