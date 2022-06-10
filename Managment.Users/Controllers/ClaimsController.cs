using System.Security.Claims;
using AutoMapper;
using Managment.Common.Models.Dtos.Requests;
using Managment.Users.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Managment.Users.Controllers;

[Controller]
[Route("api/v1/users/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin_policy")]
public class ClaimsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IClaimsRepository _claimsRepository;
    private readonly IUserRepository _userRepository;

    public ClaimsController(IMapper mapper, IClaimsRepository claimsRepository, IUserRepository userRepository)
    {
        this._mapper = mapper;
        this._claimsRepository = claimsRepository;
        this._userRepository = userRepository;
    }

    [HttpGet(Name = nameof(GetAllClaims))]
    public async Task<ActionResult> GetAllClaims(string id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return Ok(await _claimsRepository.GetUserClaimsAsync(user));
    }

    [HttpPost]
    public async Task<ActionResult> AddClaimToUser(CreateClaimRequest request)
    {
        var user = await _userRepository.GetUserByIdAsync(request.Id);

        var claim = _mapper.Map<Claim>(request);
        await _claimsRepository.AddClaimToUserAsync(user, claim);
        //не хорошо так
        return CreatedAtRoute(nameof(GetAllClaims), new
        {
            Id = user.Id
        }, claim);
    }


}