using AutoMapper;
using Managment.Users.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Managment.Common.Models.Dtos.Responses;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Managment.Common.Models.Dtos.Requests;
using Managment.Users.Services;

namespace Managment.Users.Controllers;

[Controller]
[Route("api/v1/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly JwtGenerationService _jwtGenerationService;

    public AuthenticationController(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IMapper mapper,
        TokenValidationParameters tokenValidationParameters,
        JwtGenerationService jwtGenerationService)
    {
        this._refreshTokenRepository = refreshTokenRepository;
        this._userRepository = userRepository;
        this._mapper = mapper;
        this._tokenValidationParameters = tokenValidationParameters;
        this._jwtGenerationService = jwtGenerationService;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> RefreshToken(string refreshToken){
        //Если знаете способ получше то trusenkoda@gmail.com с темой "я знаю способ лучше" 
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("bearer ","",true,null);
        var tokens = await _refreshTokenRepository.RefreshAsync(token,refreshToken);
        
        await _refreshTokenRepository.AddAsync(tokens.RefreshToken);
        await _refreshTokenRepository.SaveChangesAsync();
        
        return Ok(_mapper.Map<UserAuthenticationResponse>(tokens));
    }

    [HttpPost]
    public async Task<ActionResult> Login([FromBody] LoginUserRequest request){
        await _userRepository.CheckPasswordAsync(request.Email,request.Password);
                
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        var tokens = await _jwtGenerationService.GenerateAsync(user);

        await _refreshTokenRepository.AddAsync(tokens.RefreshToken);
        await _refreshTokenRepository.SaveChangesAsync();
        
        return Ok(_mapper.Map<UserAuthenticationResponse>(tokens));
    }
}