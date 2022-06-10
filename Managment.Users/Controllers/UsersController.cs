using AutoMapper;
using Managment.Common.Models.Dtos.Requests;
using Managment.Users.Data;
using Managment.Users.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Managment.Users.Controllers;

[Controller]
[Route("api/v1/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin_policy")]

public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IMapper _mapper;
    private readonly JwtGenerationService _jwtGenerationService;

    public UsersController(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IMapper mapper, JwtGenerationService jwtGenerationService)
    {
        this._userRepository = userRepository;
        this._refreshTokenRepository = refreshTokenRepository;
        this._mapper = mapper;
        this._jwtGenerationService = jwtGenerationService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllUsersAsync()
    {
        return Ok(await _userRepository.GetAllUsersAsync());
    }

    [HttpGet("{id}", Name = nameof(GetUserAsync))]
    public async Task<ActionResult> GetUserAsync(string id)
    {
        return Ok(await _userRepository.GetUserByIdAsync(id));
    }
    [HttpPost]
    public async Task<ActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        var newUser = _mapper.Map<IdentityUser>(request);
        await _userRepository.CreateAsync(newUser,request.Password);

        var tokens = await _jwtGenerationService.GenerateAsync(newUser);
        await _refreshTokenRepository.AddAsync(tokens.RefreshToken);
        
        await _userRepository.SaveChangesAsync();
        await _refreshTokenRepository.SaveChangesAsync();

        return CreatedAtRoute(nameof(GetUserAsync), new { Id = newUser.Id }, newUser);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUserAsync(string id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        await _userRepository.RemoveUserAsync(id);
        await _userRepository.SaveChangesAsync();
        return Ok();
    }
}