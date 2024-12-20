using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Application.Mappings;
using AuthService.Application.Services;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AutoMapper;
using Moq;

namespace AuthService.Application.Tests;

public class AuthServiceTest
{
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configuration;

    public AuthServiceTest()
    {
        _configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DomainToDTOMappingProfile>();
        });

        _mapper = _configuration.CreateMapper();
    }

    
}