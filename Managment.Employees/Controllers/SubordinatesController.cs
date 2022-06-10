using AutoMapper;
using Managment.Common.Models;
using Managment.Common.Models.Dtos.Requests;
using Managment.Common.Models.Dtos.Responses;
using Managment.Employees.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Managment.Employees.Controllers;

[Controller]
[Route("api/v1/[controller]")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin_policy")]
public class SubordinatesController : ParametrizedControllerBase<EmployeeSubordinate, EmployeeSubordinateResponse, CreateEmployeeSubordinateRequest>
{
    public SubordinatesController(IRepository<EmployeeSubordinate> repository, IMapper mapper) : base(repository, mapper)
    {
    }
}