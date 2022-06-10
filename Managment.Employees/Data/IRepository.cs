
using Managment.Common.Models;

namespace Managment.Employees.Data;

public interface IRepository<T> where T : EmployeeBase{
    Task<IEnumerable<T>> GetAllEmployeesAsync();
    Task CreateEmployeeAsync(T employee);
    Task<T> GetEmployeeByIdAsync(int employeeId);
    Task RemoveEmployeeAsync(T employee);
    Task UpdateAsync(T employee);
    Task UpgradeAsync(T employee);

    Task DowngradeAsync(T employee);

    Task SaveChangesAsync();
}