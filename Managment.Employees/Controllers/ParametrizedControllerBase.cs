using AutoMapper;
using Managment.Common.Models;
using Managment.Common.Models.Dtos.Requests;
using Managment.Common.Models.Dtos.Responses;
using Managment.Employees.Data;
using Microsoft.AspNetCore.Mvc;

namespace Managment.Employees.Controllers;

[Controller]
[Route("api/v1/[controller]")]
public class ParametrizedControllerBase<TInternal, TResponse, TReqiest> : ControllerBase
    where TInternal : EmployeeBase
    where TResponse : EmployeeBaseResponse
    where TReqiest : CreateEmployeeRequest
{
    private readonly IRepository<TInternal> _repository;
    private readonly IMapper _mapper;

    public ParametrizedControllerBase(IRepository<TInternal> repository, IMapper mapper)
    {
        this._repository = repository;
        this._mapper = mapper;
    }

    [HttpGet
    //(Name = "[controller]/" + nameof(GetAllEmployeesAsync) + nameof(TResponse))
    ]
    public async Task<ActionResult> GetAllEmployeesAsync()
    {
        return Ok(_mapper.Map<IEnumerable<TResponse>>(await _repository.GetAllEmployeesAsync()));
    }

    [HttpPost
    //(Name = "[controller]/" + nameof(CreateEmployeeAsync) + nameof(TResponse))
    ]
    public async Task<ActionResult> CreateEmployeeAsync([FromBody] TReqiest request)
    {
        var employee = _mapper.Map<TInternal>(request);
        await _repository.CreateEmployeeAsync(employee);
        await _repository.SaveChangesAsync();
        //return CreatedAtRoute(nameof(GetEmployeeByIdAsync), new { Id = employee.Id }, _mapper.Map<TResponse>(employee));
        return Ok(_mapper.Map<TResponse>(employee));
    }
    [HttpDelete
    //(Name = "[controller]/" + nameof(RemoveEmployeeAsync) + nameof(TResponse))
    ]
    public async Task<ActionResult> RemoveEmployeeAsync(int id)
    {
        var employee = await _repository.GetEmployeeByIdAsync(id);
        await _repository.RemoveEmployeeAsync(employee);
        await _repository.SaveChangesAsync();
        return Ok();
    }

    [HttpGet, Route("{id}"
    //, Name = "[controller]/" + nameof(GetEmployeeByIdAsync) + nameof(TResponse)
    )]
    public async Task<ActionResult> GetEmployeeByIdAsync(int id)
    {
        var employee = await _repository.GetEmployeeByIdAsync(id);
        return Ok(_mapper.Map<TResponse>(employee));
    }


    /*
        повышает employee до subordinate
        повышает subordinate до manager
    */
    [HttpPost, Route("upgrade/{id}"
    //, Name = "[controller]/" + nameof(UpgradeEmployeeAsync) + nameof(TResponse)
    )]
    public async Task<ActionResult> UpgradeEmployeeAsync(int id)
    {
        var employee = await _repository.GetEmployeeByIdAsync(id);
        await _repository.UpgradeAsync(employee);
        await _repository.SaveChangesAsync();
        return Ok();
    }

    /*
        понижает manager до subordinate
        понижает subordinate до employee
    */
    [HttpPost, Route("downgrade/{id}"
    //, Name = "[controller]/" + nameof(DowngradeEmployeeAsync) + nameof(TResponse)
    )]
    public async Task<ActionResult> DowngradeEmployeeAsync(int id)
    {
        var employee = await _repository.GetEmployeeByIdAsync(id);
        await _repository.DowngradeAsync(employee);
        await _repository.SaveChangesAsync();
        return Ok();
    }

    [HttpPost, Route("update/{id}"
    //, Name = "[controller]/" + nameof(UpdateEmployeeAsync) + nameof(TResponse)
    )]
    public async Task<ActionResult> UpdateEmployeeAsync(int id, [FromBody] TReqiest request)
    {
        var employee = await _repository.GetEmployeeByIdAsync(id);
        await _repository.UpdateAsync(employee);
        await _repository.SaveChangesAsync();
        return Ok();
    }

}