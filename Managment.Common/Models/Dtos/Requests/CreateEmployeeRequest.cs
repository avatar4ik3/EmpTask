using System.ComponentModel.DataAnnotations;

namespace Managment.Common.Models.Dtos.Requests;

public class CreateEmployeeRequest
{
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
    
    public string Department { get; set; }
}