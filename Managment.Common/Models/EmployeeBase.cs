using System.ComponentModel.DataAnnotations;

namespace Managment.Common.Models;

public class EmployeeBase
{
    [Key]
    public int Id{get;set;}
    [Required]
    public string Name{get;set;}

    [Required]
    [EmailAddress]
    public string Email {get;set;}
    [Required]
    public Sex Sex{get;set;}
    
    [Required]
    public DateTime DateOfBirth {get;set;}

    [Required]
    public string Post {get;set;}

    [Required]
    public int Salary{get;set;}
    [Required]
    public string Department {get;set;}
}