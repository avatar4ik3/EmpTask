using AutoMapper;
using Managment.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Managment.Employees.Data;

public class EmployeeBaseRepository : RepositoryBase<EmployeeBase>
{
    private readonly IMapper _mapper;
    public EmployeeBaseRepository(EmployeesDbContext dbContext,IMapper mapper) : base(dbContext, dbContext.Employees)
    {
        this._mapper = mapper;
    }

    public override Task DowngradeAsync(EmployeeBase employee)
    {
        throw new BadHttpRequestException($"Cannot Downgrade {nameof(EmployeeBase)}");
    }
    
    public override async Task UpgradeAsync(EmployeeBase employee)
    {
        //ничего лучше я не придумал, а времени лекции смотреть не осталось
        var employeeSubordinate = _mapper.Map<EmployeeSubordinate>(employee);
        await RemoveEmployeeAsync(employee);
        await _dbContext.SaveChangesAsync();
        _dbContext.Subordinates.Add(employeeSubordinate);
        
    }
}