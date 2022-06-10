using System.Security.Claims;
using AutoMapper;
using Managment.Common.Models.Dtos.Requests;
using Managment.Common.Models.Dtos.Responses;
using Managment.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace Managment.Users.Profiles;

public class AppProfile : Profile
{
    public AppProfile()
    {
        CreateMap<TokenAndRefreshToken,UserAuthenticationResponse>()
            .ForMember(
                d => d.RefreshToken,
                options => options.MapFrom(s => s.RefreshToken.Token));
        CreateMap<AddUserToRoleRequest,IdentityRole>()
            .ConstructUsing((s,context) => new IdentityRole(s.Role));
        CreateMap<CreateUserRequest,IdentityUser>();
        CreateMap<CreateClaimRequest,Claim>()
            .ConstructUsing((s,context) => new Claim(s.Name,s.Value));
        CreateMap<CreateUserRequest,IdentityUser>()
            .ConstructUsing((s,context) => new IdentityUser(){UserName = s.UserName,Email = s.Email});
    }
}