using Managment.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Managment.Employees.Data;

public abstract class RepositoryBase<TModel> : IRepository<TModel>
    where TModel : EmployeeBase
{
    protected readonly EmployeesDbContext _dbContext;
    protected readonly DbSet<TModel> _dbSet;

    public RepositoryBase(EmployeesDbContext dbContext, DbSet<TModel> dbSet)
    {
        this._dbContext = dbContext;
        this._dbSet = dbSet;
    }
    public async Task CreateEmployeeAsync(TModel employee)
    {
        var result = await _dbSet.AddAsync(employee);
        if ((result.State == EntityState.Added) is false)
        {
            throw new BadHttpRequestException($"Unale To add employee");
        }
    }


    public async Task<IEnumerable<TModel>> GetAllEmployeesAsync()
    {
        return _dbSet.ToList();
    }

    public async Task<TModel> GetEmployeeByIdAsync(int employeeId)
    {
        var result = await _dbSet.FindAsync(employeeId);
        if (result is null)
        {
            throw new BadHttpRequestException($"Employee with given id {employeeId} does not exists");
        }
        return result!;
    }

    public async Task RemoveEmployeeAsync(TModel employee)
    {
        var result = _dbSet.Remove(employee);
        if ((result.State == EntityState.Deleted || result.State == EntityState.Detached) is false)
        {
            throw new BadHttpRequestException($"Unable to remove employee with id {employee.Id}");
        }
    }


    public async Task UpdateAsync(TModel employee)
    {

        var result = _dbSet.Update(employee);
        if ((result.State == EntityState.Modified || result.State == EntityState.Unchanged) is false)
        {
            throw new BadHttpRequestException($"Unable to modify employee with id {employee.Id}");
        }

    }

    public abstract Task DowngradeAsync(TModel employee);
    public abstract Task UpgradeAsync(TModel employee);

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}