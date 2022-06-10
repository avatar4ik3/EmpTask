using System.ComponentModel.DataAnnotations;

namespace Managment.Common.Models.Dtos.Requests;

public class CreateRoleRequest
{
    [Required]
    public string Role { get; set; }
}