using Managment.Common.Models;

namespace Managment.Employees.Data;

public class InMemoryDbSeeder
{
    private readonly EmployeesDbContext _dbContext;
    public InMemoryDbSeeder(EmployeesDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task SeedDb(){
        var empM = new EmployeeManager{
            Name = "Ivan",
            Email = "Ivan@Ivan.com",
            Sex = Sex.Male,
            DateOfBirth = DateTime.UtcNow,
            Post = "начальник отдела",
            Salary = 0,
            Department = "Рабочий отдел"
        };
        var empE = new EmployeeBase{
            Name = "Denis",
            Email = "Denis@Denis.com",
            Sex = Sex.Male,
            DateOfBirth = DateTime.UtcNow,
            Post = "простой рабочий",
            Salary = 0,
            Department = "Рабочий отдел"
        };
        
        _dbContext.Employees.Add(empE);
        _dbContext.Managers.Add(empM);

        var empS = new EmployeeSubordinate{
            Name = "Denis",
            Email = "Denis@Denis.com",
            Sex = Sex.Male,
            DateOfBirth = DateTime.UtcNow,
            Post = "простой рабочий",
            Salary = 0,
            Department = "Рабочий отдел",
            ManagerId = empE.Id
        };

        _dbContext.SaveChanges();        
        empM.Subordinates = new List<EmployeeSubordinate>(new[]{empS});
        _dbContext.Managers.Update(empM);
        _dbContext.Subordinates.Add(empS);
        _dbContext.SaveChanges();        
    }
    
}