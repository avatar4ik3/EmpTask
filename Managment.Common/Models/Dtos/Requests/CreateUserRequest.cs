using System.ComponentModel.DataAnnotations;

namespace Managment.Common.Models.Dtos.Requests;

public class CreateUserRequest : LoginUserRequest
{
    [Required]
    public string UserName { get; set; }
}