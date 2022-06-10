using AutoMapper;
using Managment.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Managment.Employees.Data;

public class ManagerRepository : RepositoryBase<EmployeeManager>
{
    private readonly IMapper _mapper;

    public ManagerRepository(EmployeesDbContext dbContext, IMapper mapper) : base(dbContext, dbContext.Managers)
    {
        this._mapper = mapper;
    }

    public override async Task DowngradeAsync(EmployeeManager employee)
    {
        var employeeSubordinate = _mapper.Map<EmployeeSubordinate>(employee);
        await RemoveEmployeeAsync(employee);
        await _dbContext.SaveChangesAsync();
        _dbContext.Subordinates.Add(employeeSubordinate);
    }

    public override Task UpgradeAsync(EmployeeManager employee)
    {
        throw new BadHttpRequestException($"Cannot Upgrade {nameof(ManagerRepository)}");
    }
}