namespace Managment.Common.Models;

public class EmployeeSubordinate : EmployeeBase
{
    public int ManagerId {get;set;}

    public EmployeeManager Manager{get;set;}
}