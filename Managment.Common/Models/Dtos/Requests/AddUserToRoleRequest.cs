using System.ComponentModel.DataAnnotations;

namespace Managment.Common.Models.Dtos.Requests;

public class AddUserToRoleRequest
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Role { get; set; }
}