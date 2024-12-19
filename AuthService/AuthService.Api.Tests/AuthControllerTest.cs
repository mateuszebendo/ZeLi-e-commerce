using AuthService.Api.Controllers;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Api.Tests;

public class AuthControllerTest
{
    private readonly Mock<IUsuarioService> _mockService;
    private readonly AuthController _clienteController;
    private readonly IMapper _mapper;
    private readonly Mock<ITokenService> _mockToken;

    public AuthControllerTest()
    {
        _mockService = new Mock<IUsuarioService>();
        _mockToken = new Mock<ITokenService>();
        _clienteController = new AuthController(_mockService.Object, _mapper, _mockToken.Object);
    }
}


