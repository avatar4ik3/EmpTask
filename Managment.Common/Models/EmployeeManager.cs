namespace Managment.Common.Models;

public class EmployeeManager : EmployeeBase
{
    public ICollection<EmployeeSubordinate> Subordinates {get;set;}
}