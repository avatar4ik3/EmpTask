using AutoMapper;
using Managment.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Managment.Employees.Data;

public class SubordinateRepository : RepositoryBase<EmployeeSubordinate>
{
    private readonly IMapper _mapper;


    public SubordinateRepository(EmployeesDbContext dbContext, IMapper mapper) : base(dbContext, dbContext.Subordinates)
    {
        this._mapper = mapper;
    }

    public override async Task DowngradeAsync(EmployeeSubordinate employee)
    {
        var employeeBase = _mapper.Map<EmployeeBase>(employee);
        await RemoveEmployeeAsync(employee);
        await _dbContext.SaveChangesAsync();
        _dbContext.Employees.Add(employeeBase);
    }

    public override async Task UpgradeAsync(EmployeeSubordinate employee)
    {
        var employeeManager = _mapper.Map<EmployeeManager>(employee);
        await RemoveEmployeeAsync(employee);
        await _dbContext.SaveChangesAsync();
        _dbContext.Managers.Add(employeeManager);
    }
}