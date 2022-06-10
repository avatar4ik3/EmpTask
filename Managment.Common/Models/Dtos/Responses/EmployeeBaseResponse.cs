namespace Managment.Common.Models.Dtos.Responses;


public class EmployeeBaseResponse
{
    public int Id {get;set;}
    public string Name{get;set;}

    public string Email {get;set;}

    public Sex Sex{get;set;}
    
    public DateTime DateOfBirth {get;set;}

    public string Post {get;set;}
    public int Salary{get;set;}
    public string Department {get;set;}
}