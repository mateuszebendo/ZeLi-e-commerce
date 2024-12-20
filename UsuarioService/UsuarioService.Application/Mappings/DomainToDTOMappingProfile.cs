using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsuarioService.Application.DTOs;
using UsuarioService.Domain.Entities;

namespace UsuarioService.Application.Mappings;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Usuario, UsuarioDto>().ReverseMap();
        CreateMap<Usuario, UsuarioCreateDto>().ReverseMap();
        CreateMap<Usuario, UsuarioUpdateDto>().ReverseMap();
    }
}
