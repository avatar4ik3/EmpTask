using System.ComponentModel.DataAnnotations;

namespace Managment.Common.Models.Dtos.Requests;

public class LoginUserRequest
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password {get;set;} 
}