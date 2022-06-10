using AutoMapper;
using Managment.Common.Models.Dtos.Requests;
using Managment.Users.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Managment.Users.Controllers;


[Controller]
[Route("api/v1/users/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin_policy")]
public class RolesController : ControllerBase
{
    private readonly IRolesRepository _rolesRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public RolesController(IRolesRepository rolesRepository, IUserRepository userRepository,IMapper mapper)
    {
        this._rolesRepository = rolesRepository;
        this._userRepository = userRepository;
        this._mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllRolesAsync(){
        return Ok(await _rolesRepository.GetAllRolesAsync());
    }

    [HttpPost]
    public async Task<ActionResult> CreateRoleAsync([FromBody] CreateRoleRequest request){
        var role = await _rolesRepository.CreateAsync(request.Role);
        await _rolesRepository.SaveChangesAsync();
        return CreatedAtRoute(nameof(GetRoleAsync),new{Id = role.Id},role);
    }

    [HttpGet("{id}",Name =nameof(GetRoleAsync))]
    public async Task<ActionResult> GetRoleAsync(string roleId){
        return Ok(await _rolesRepository.GetRoleAsync(roleId));
    }

    [HttpPost("users")]
    public async Task<ActionResult> AddUserToRoleAsync([FromBody] AddUserToRoleRequest request){
        var user = await _userRepository.GetUserByIdAsync(request.Id);
        var role = _mapper.Map<IdentityRole>(request);
        await _rolesRepository.AddUserToRole(user,role);
        return Ok();
    }

    [HttpGet("users/{id}")]
    public async Task<ActionResult> GetUserRolesAsync(string userId){
        var user = await _userRepository.GetUserByIdAsync(userId);
        return Ok(await _rolesRepository.GetUserRolesAsync(user));
    }
}