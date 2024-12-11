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
        CreateMap<Cliente, ClienteDto>().ReverseMap();
        CreateMap<Cliente, ClienteCreateDto>().ReverseMap();
    }
}
